using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterBot.Framework.Contracts;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Contracts.ServiceBus;
using TwitterBot.Framework.Exceptions;
using TwitterBot.Framework.Types;

namespace TwitterBot.AzureFunctions
{
    public class TweetBotFunction
    {
        private readonly ITweetOperations _tweetOperations;
        private readonly IServiceBusOperations _serviceBusOperations;
        private readonly IDocumentDbRepository<Tweet> _tweetDbRepository;
        private readonly IDocumentDbRepository<Hashtag> _hashtagDbRepository;

        public TweetBotFunction(
            ITweetOperations tweetOperations, 
            IServiceBusOperations serviceBusOperations,
            IDocumentDbRepository<Tweet> tweetDbRepository,
            IDocumentDbRepository<Hashtag> hashtagDbRepository)
        {
            _tweetOperations = tweetOperations;
            _serviceBusOperations = serviceBusOperations;
            _tweetDbRepository = tweetDbRepository;
            _hashtagDbRepository = hashtagDbRepository;
        }

        [FunctionName("TweetBotFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var hashTagMessagess = await _serviceBusOperations.ReceiveMessagesAsync();
            var hashTags = hashTagMessagess.Select(p => JsonConvert.DeserializeObject<Hashtag>(Encoding.UTF8.GetString(p.Body)));
            var erroredHashtags = new List<Hashtag>();

            foreach (var hashTag in hashTags)
            {
                try
                {
                    var tweet = _tweetOperations.GetPopularTweetByHashtag(hashTag);

                    if (tweet != null)
                    {
                        tweet.Hashtags = new List<Hashtag>();
                        log.LogInformation($"Latest popular tweet for {hashTag.Text} : {tweet.FullText}");

                        var existingTweet = await _tweetDbRepository.GetByIdAsync(tweet.Id);
                        if (existingTweet is null)
                        {
                            tweet.Hashtags.Add(hashTag);
                            await _tweetDbRepository.AddOrUpdateAsync(tweet);

                            log.LogInformation($"Added Tweet in TweetCollection with Id: { tweet.Id }");
                        }

                        if (existingTweet != null && !existingTweet.Hashtags.Any(p => p.Text == hashTag.Text))
                        {
                            tweet.Hashtags = existingTweet.Hashtags;
                            tweet.Hashtags.Add(hashTag);

                            await _tweetDbRepository.AddOrUpdateAsync(tweet);
                            log.LogInformation($"Updated Tweet in TweetCollection with Id: { tweet.Id }");
                        }
                    }

                    hashTag.IsCurrentlyInQueue = false;
                    hashTag.LastSyncedDateTime = DateTime.UtcNow;

                    await _hashtagDbRepository.AddOrUpdateAsync(hashTag);
                }
                catch (Exception ex)
                {
                    log.LogError(ex, ex.Message);
                    erroredHashtags.Add(hashTag);
                }
            }

            if (erroredHashtags.Any())
            {
                throw new TwitterBotBusinessException(erroredHashtags);
            }
        }
    }
}

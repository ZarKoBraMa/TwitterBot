using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterBot.Framework.Contracts;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Types;

namespace TwitterBot.AzureFunctions
{
    public class TweetBotFunction
    {
        private readonly ITweetOperations _tweetOperations;
        private readonly IDocumentDbRepository<Tweet> _tweetDbRepository;

        public TweetBotFunction(ITweetOperations tweetOperations, IDocumentDbRepository<Tweet> tweetDbRepository)
        {
            _tweetOperations = tweetOperations;
            _tweetDbRepository = tweetDbRepository;
        }

        [FunctionName("TweetBotFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var hashTag = new Hashtag { Text = "#justsaying"};
            var tweet = _tweetOperations.GetPopularTweetByHashtag(hashTag);

            if (tweet != null)
            {
                tweet.Hashtags = new List<Hashtag>();
                log.LogInformation($"Latest popular tweet for #justsaying : {tweet.FullText}");

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
        }
    }
}

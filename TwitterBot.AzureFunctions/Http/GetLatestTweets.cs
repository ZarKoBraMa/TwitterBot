using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Types;
using System.Linq;
using TwitterBot.AzureFunctions.Configurations;
using Microsoft.Extensions.Options;

namespace TwitterBot.AzureFunctions.Http
{
    public class GetLatestTweets
    {
        private readonly IDocumentDbRepository<User> _userRepository;
        private readonly IDocumentDbRepository<Tweet> _tweetRepository;
        private readonly IOptions<AppSettingsConfiguration> _configurations;

        public GetLatestTweets(
            IDocumentDbRepository<User> userRepository, 
            IDocumentDbRepository<Tweet> tweetRepository,
            IOptions<AppSettingsConfiguration> configurations)
        {
            _userRepository = userRepository;
            _tweetRepository = tweetRepository;
            _configurations = configurations;
        }

        [FunctionName("GetLatestTweets")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetLatestTweets started.");

            string userId = req.Query["uid"];

            var dbUsers = await _userRepository.TopAsync(p => p.UserId == userId, 1);
            if (dbUsers == null || dbUsers.Count() == 0)
            {
                return new JsonResult(null);
            }

            var user = dbUsers.ToList().FirstOrDefault(p => p.UserId == userId);

            if (user.Hashtags == null || user.Hashtags.Count == 0)
            {
                return new JsonResult(null);
            }

            var tweets = _tweetRepository.GetTweetsByHashtags(
                user.Hashtags.Select(p => p.Text).ToArray(), 
                DateTime.UtcNow.AddDays(_configurations.Value.AppSettings.TweetsFilterIntervalInDays));
            
            if (tweets != null)
            {
                tweets = tweets.OrderByDescending(p => p.TweetCreatedOn);
            }
   
            log.LogInformation("GetLatestTweets completed.");
            return new JsonResult(tweets);
        }
    }
}

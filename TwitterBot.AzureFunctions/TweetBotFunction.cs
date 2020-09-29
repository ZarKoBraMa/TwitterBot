using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TwitterBot.Framework.Contracts;

namespace TwitterBot.AzureFunctions
{
    public class TweetBotFunction
    {
        private readonly ITweetOperations _tweetOperations;

        public TweetBotFunction(ITweetOperations tweetOperations)
        {
            _tweetOperations = tweetOperations;
        }

        [FunctionName("TweetBotFunction")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {   
            var tweet = _tweetOperations.GetPopularTweetByHashtag(new Framework.Types.Hashtag
            {
                Text = "#justsaying"
            });

            if (tweet != null)
            {
                log.LogInformation($"Latest popular tweet for #justsaying : {tweet.FullText}");
            }
        }
    }
}

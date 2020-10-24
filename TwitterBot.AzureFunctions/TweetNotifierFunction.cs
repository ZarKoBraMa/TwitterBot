using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using TwitterBot.Framework.Contracts.Data;
using Types = TwitterBot.Framework.Types;

namespace TwitterBot.AzureFunctions
{
    public class TweetNotifierFunction
    {
        private readonly IDocumentDbRepository<Types.User> _userRepository;
        private readonly IDocumentDbRepository<Types.Tweet> _tweetRepository;

        public TweetNotifierFunction(IDocumentDbRepository<Types.User> userRepository, IDocumentDbRepository<Types.Tweet> tweetRepository)
        {
            _userRepository = userRepository;
            _tweetRepository = tweetRepository;
        }

        [FunctionName("TweetNotifierFunction")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "TwitterBotDB",
            collectionName: "TweetCollection",
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Document> documents,
            [SignalR(
                HubName = "TweetNotificationsHub", 
                ConnectionStringSetting = "SignalR:ConnectionString")] IAsyncCollector<SignalRMessage> messages,
            ILogger log)
        {
            log.LogInformation("Documents modified " + documents.Count);

            foreach (var document in documents)
            {
                var tweet = await _tweetRepository.GetByIdAsync(document.Id);
                var users = _userRepository.GetUsersByHashtags(tweet.Hashtags.Select(p => p.Text).ToArray());

                foreach (var user in users)
                {
                    await messages.AddAsync(new SignalRMessage
                    {
                        UserId = user.Id,
                        Target = "updateTweets",
                        Arguments = new[] { tweet }
                    });
                }
            }

            await messages.FlushAsync();
        }
    }
}

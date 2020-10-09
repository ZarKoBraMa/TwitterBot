using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TwitterBot.AzureFunctions
{
    public static class TweetNotifierFunction
    {
        [FunctionName("TweetNotifierFunction")]
        public static void Run([CosmosDBTrigger(
            databaseName: "TwitterBotDB",
            collectionName: "TweetCollection",
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}

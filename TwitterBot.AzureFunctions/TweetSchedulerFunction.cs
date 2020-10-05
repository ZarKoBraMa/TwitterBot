using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Contracts.ServiceBus;
using TwitterBot.Framework.Types;

namespace TwitterBot.AzureFunctions
{
    public class TweetSchedulerFunction
    {
        private readonly IServiceBusOperations _serviceBusOperations;
        private readonly IDocumentDbRepository<Hashtag> _hashTagRepository;

        public TweetSchedulerFunction(IServiceBusOperations serviceBusOperations, IDocumentDbRepository<Hashtag> hashTagRepository)
        {
            _serviceBusOperations = serviceBusOperations;
            _hashTagRepository = hashTagRepository;
        }

        [FunctionName("TweetSchedulerFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"TweetSchedulerFunction started executionat: { DateTime.Now}");

            var hashTags = await _hashTagRepository.WhereAsync(p => 
                (!p.IsCurrentlyInQueue && p.LastSyncedDateTime < DateTime.UtcNow.AddMinutes(-10))
                    || (p.IsCurrentlyInQueue && p.LastSyncedDateTime < DateTime.UtcNow.AddHours(-1)));

            foreach (var hashTag in hashTags)
            {   
                await _serviceBusOperations.SendMessageAsync(hashTag.Id, JsonConvert.SerializeObject(hashTag));
                
                hashTag.IsCurrentlyInQueue = true;
                await _hashTagRepository.AddOrUpdateAsync(hashTag);
            }

            log.LogInformation($"TweetSchedulerFunction completed execution at: { DateTime.Now}");
        }
    }
}

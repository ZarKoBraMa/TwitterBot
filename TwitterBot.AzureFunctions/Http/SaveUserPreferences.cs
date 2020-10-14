using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Types;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TwitterBot.AzureFunctions.Http
{
    public class SaveUserPreferences
    {
        private readonly IDocumentDbRepository<User> _userRepository;
        private readonly IDocumentDbRepository<Hashtag> _hashTagRepository;

        public SaveUserPreferences(IDocumentDbRepository<User> userRepository, IDocumentDbRepository<Hashtag> hashTagRepository)
        {
            _userRepository = userRepository;
            _hashTagRepository = hashTagRepository;
        }

        [FunctionName("SaveUserPreferences")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SaveUserPreferences started.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<User>(requestBody);

            var hashTags = user.Hashtags != null ? user.Hashtags.Select(p => p.Text).ToList() : new List<string>();
            var dbHashtagQuery = await _hashTagRepository.WhereAsync(p => hashTags.Contains(p.Text));
            var dbHashtags = dbHashtagQuery.ToList();

            foreach (var hashtag in user.Hashtags)
            {
                if (dbHashtags.Any(p => p.Text == hashtag.Text))
                {
                    continue;
                }

                hashtag.IsCurrentlyInQueue = false;
                hashtag.LastSyncedDateTime = DateTime.UtcNow.AddMinutes(-10);
                await _hashTagRepository.AddOrUpdateAsync(hashtag);
            }

            var dbUserQuery = await _userRepository.WhereAsync(p => p.UserId == user.UserId);
            var users = dbUserQuery.ToList();

            if (users != null && users.Count() != 0)
            {
                user.Id = users.FirstOrDefault().Id;
            }

            await _userRepository.AddOrUpdateAsync(user);

            log.LogInformation("SaveUserPreferences completed.");
            return new OkResult();
        }
    }
}

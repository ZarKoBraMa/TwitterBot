using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Types;
using System.Linq;

namespace TwitterBot.AzureFunctions.Http
{
    public class RetrieveUserPreferences
    {
        private readonly IDocumentDbRepository<User> _userRepository;

        public RetrieveUserPreferences(IDocumentDbRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [FunctionName("RetrieveUserPreferences")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("RetrieveUserPreferences started.");

            string userId = req.Query["uid"];
            var users = await _userRepository.TopAsync(p => p.UserId == userId, 1);
            
            if (users == null || users.Count() == 0)
            {
                return new JsonResult(null);
            }

            var user = users.ToList().FirstOrDefault(p => p.UserId == userId);
            
            log.LogInformation("RetrieveUserPreferences completed.");
            return new JsonResult(user);
        }
    }
}

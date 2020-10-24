using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace TwitterBot.AzureFunctions.Http
{
    public static class SignalRConnection
    {
        [FunctionName("SignalRConnection")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalRConnectionInfo(
                HubName = "TweetNotificationsHub", 
                UserId = "{headers.x-userid}",  
                ConnectionStringSetting = "SignalR:ConnectionString")]
             SignalRConnectionInfo connectionInfo)
        {   
            return new OkObjectResult(connectionInfo);
        }
    }
}

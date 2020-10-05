using Microsoft.Azure.ServiceBus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwitterBot.Framework.Contracts.ServiceBus
{
    public interface IServiceBusOperations
    {
        Task SendMessageAsync(string id, string message);
        Task<List<Message>> ReceiveMessagesAsync();
    }
}

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitterBot.Framework.Contracts.ServiceBus;

namespace TwitterBot.Framework.ServiceBus
{
    public class ServiceBusOperations : IServiceBusOperations
    {
        private IServiceBusContext _serviceBusContext;
        private IMessageSender _messageSender;
        private ISessionClient _sessionClient;

        public ServiceBusOperations(IServiceBusContext serviceBusContext)
        {
            _serviceBusContext = serviceBusContext;
            _messageSender = new MessageSender(serviceBusContext.ConnectionString, serviceBusContext.QueueName);
            _sessionClient = new SessionClient(serviceBusContext.ConnectionString, serviceBusContext.QueueName);
        }

        public async Task<List<Message>> ReceiveMessagesAsync()
        {
            var messages = new List<Message>();
            IMessageSession session = await _sessionClient.AcceptMessageSessionAsync(_serviceBusContext.SessionId);
            
            if (session == null)
            {
                return messages;
            }
            
            for (int i = 0; i < _serviceBusContext.MaxConcurrentMessagesToBeRetrieved; i++)
            {
                Message message = await session.ReceiveAsync(_serviceBusContext.OperationTimeout);
                if (message == null)
                {
                    break;
                }

                messages.Add(message);                
                await session.CompleteAsync(message.SystemProperties.LockToken);
            }
            
            await session.CloseAsync();
            return messages;
        }

        public async Task SendMessageAsync(string id, string message)
        {
            await _messageSender.SendAsync(new Message(Encoding.UTF8.GetBytes(message))
            {
                MessageId = id,
                SessionId = _serviceBusContext.SessionId
            });
        }
    }
}

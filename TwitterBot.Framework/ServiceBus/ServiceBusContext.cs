using System;
using TwitterBot.Framework.Contracts.ServiceBus;

namespace TwitterBot.Framework.ServiceBus
{
    public class ServiceBusContext : IServiceBusContext
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public string SessionId { get; set; }
        public int MaxConcurrentMessagesToBeRetrieved { get; set; }
        public TimeSpan OperationTimeout { get; set; }
    }
}

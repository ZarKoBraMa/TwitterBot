using System;

namespace TwitterBot.Framework.Contracts.ServiceBus
{
    public interface IServiceBusContext
    {
        string ConnectionString { get; set; }
        string QueueName { get; set; }
        string SessionId { get; set; }
        int MaxConcurrentMessagesToBeRetrieved { get; set; }
        TimeSpan OperationTimeout { get; set; }
    }
}

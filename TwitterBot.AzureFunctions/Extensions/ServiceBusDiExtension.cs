using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TwitterBot.AzureFunctions.Common;
using TwitterBot.Framework.Contracts.ServiceBus;
using TwitterBot.Framework.ServiceBus;

namespace TwitterBot.AzureFunctions.Extensions
{
    public static class ServiceBusDiExtension
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusContext = new ServiceBusContext()
            {
                ConnectionString = configuration[Constants.SERVICE_BUS_CONNECTION_STRING],
                QueueName = configuration[Constants.SERVICE_BUS_QUEUE_NAME],
                MaxConcurrentMessagesToBeRetrieved = 2,
                SessionId = "TwitterBotApplication",
                OperationTimeout = TimeSpan.FromMilliseconds(500)
            };

            services
                .AddSingleton<IServiceBusContext>(serviceBusContext)
                .AddSingleton<IServiceBusOperations>(new ServiceBusOperations(serviceBusContext));

            return services;
        }
    }
}

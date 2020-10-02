using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TwitterBot.AzureFunctions.Common;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.CosmosDB;

namespace TwitterBot.AzureFunctions.Configurations
{
    public static class CosmosDbDiExtension
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            var documentDbContext = new DocumentDbContext
            {
                AuthKey = configuration[Constants.COSMOS_DB_API_KEY],
                DatabaseId = configuration[Constants.COSMOS_DB_DATABASE_ID],
                EndpointUri = configuration[Constants.COSMOS_DB_ENDPOINT_URI]
            };

            Task.Run(async () => await documentDbContext
                .CreateDatabaseAndCollectionsAsync())
                .Wait();

            services
                .AddSingleton<IDocumentDbContext>(documentDbContext)
                .AddSingleton(typeof(IDocumentDbRepository<>), typeof(DocumentDbRepository<>));

            return services;
        }
    }
}

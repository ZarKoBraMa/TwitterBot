using AspNetCore.Identity.DocumentDb;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterBot.Web.Common;
using TwitterBot.Web.Identity;

namespace TwitterBot.Web.Configurations
{
    public static class CosmosDbIdentity
    {
        public static IServiceCollection AddCosmosDbIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseName = configuration[Constants.COSMOS_DB_DATABASE];
            var collectionName = configuration[Constants.COSMOS_DB_COLLECTION];

            var serializationSettings = new JsonSerializerSettings();
            serializationSettings.Converters.Add(new JsonClaimConverter());

            var documentClient = new DocumentClient(
                new Uri(configuration[Constants.COSMOS_DB_ENDPOINT_URI]),
                configuration[Constants.COSMOS_DB_AUTH_KEY],
                serializationSettings);

            services.AddSingleton<IDocumentClient>(documentClient);

            Task.Run(async () => await InitializeDatabaseAsync(documentClient, databaseName)).Wait();
            Task.Run(async () => await InitializeCollectionAsync(documentClient, databaseName, collectionName)).Wait();

            services
                .AddIdentity<ApplicationUser, DocumentDbIdentityRole>()
                .AddDocumentDbStores(options =>
                {
                    options.UserStoreDocumentCollection = collectionName;
                    options.Database = databaseName;
                });

            return services;
        }

        private async static Task InitializeDatabaseAsync(DocumentClient documentClient, string databaseName)
        {
            await documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
        }

        private async static Task InitializeCollectionAsync(DocumentClient documentClient, string databaseName, string collectionName)
        {
            DocumentCollection collection = new DocumentCollection { Id = collectionName };
            await documentClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(databaseName), 
                collection);
        }
    }
}

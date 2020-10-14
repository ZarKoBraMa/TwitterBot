using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Types;
using User = TwitterBot.Framework.Types.User;

namespace TwitterBot.Framework.CosmosDB
{
    public class DocumentDbContext : IDocumentDbContext
    {
        private IDocumentClient _documentClient;
        private IList<IDocumentDbEntity> _documentDbEntities;

        public string DatabaseId { get; set; }
        public string EndpointUri { get; set; }
        public string AuthKey { get; set; }

        public IDocumentClient DocumentClient 
        {
            get
            {
                if (_documentClient is null)
                {
                    _documentClient = GetDocumentClient();
                }
                return _documentClient;
            }
        }

        public ICollection<IDocumentDbEntity> EntityCollection
        {
            get 
            {
                if (_documentDbEntities is null)
                {
                    _documentDbEntities = GetDocumentEntities();
                }
                return _documentDbEntities;
            }
        }

        public async Task CreateDatabaseAndCollectionsAsync()
        {
            await CreateDatabaseAsync(DatabaseId);
            foreach (var entity in EntityCollection)
            {
                await CreateCollectionAsync(DatabaseId, entity.Name);
            }
        }

        #region Helper Methods
        private IDocumentClient GetDocumentClient()
        {
            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Gateway,
                ConnectionProtocol = Protocol.Https,
                MaxConnectionLimit = 1000,
                RetryOptions = new RetryOptions
                {
                    MaxRetryAttemptsOnThrottledRequests = 3,
                    MaxRetryWaitTimeInSeconds = 30
                },
                EnableEndpointDiscovery = true,
                EnableReadRequestsFallback = true
            };

            connectionPolicy.PreferredLocations.Add(LocationNames.NorthEurope);
            var client = new DocumentClient(new Uri(EndpointUri), AuthKey, connectionPolicy);
            
            return client;
        }

        private IList<IDocumentDbEntity> GetDocumentEntities()
        {
            var entityCollection = new List<IDocumentDbEntity>()
            {
                new DocumentDbEntity { EntityType = typeof(Tweet), Name = "TweetCollection" },
                new DocumentDbEntity { EntityType = typeof(Hashtag), Name ="HashtagCollection" },
                new DocumentDbEntity { EntityType = typeof(User), Name = "UserCollection" }
            };
            return entityCollection;
        }

        private async Task<Database> CreateDatabaseAsync(string databaseId)
        {
            var response = await DocumentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
            return response.Resource;
        }

        private async Task<DocumentCollection> CreateCollectionAsync(string databaseId, string collectionName)
        {
            var response = await DocumentClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(databaseId),
                new DocumentCollection
                {
                    Id = collectionName
                },
                new RequestOptions());

            return response.Resource;
        }
        #endregion
    }
}

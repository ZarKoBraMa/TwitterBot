namespace TwitterBot.AzureFunctions.Common
{
    public static class Constants
    {
        public const string TWITTER_API_KEY = "TwitterApi:Key";
        public const string TWITTER_API_SECRET = "TwitterApi:Secret";

        public const string COSMOS_DB_API_KEY = "CosmosDB:AuthKey";
        public const string COSMOS_DB_DATABASE_ID = "DatabaseId";
        public const string COSMOS_DB_ENDPOINT_URI = "CosmosDB:EndpointUri";

        public const string SERVICE_BUS_CONNECTION_STRING = "ServiceBus:ConnectionString";
        public const string SERVICE_BUS_QUEUE_NAME = "TwitterBotServiceBus_QueueName";
    }
}

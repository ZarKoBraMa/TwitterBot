namespace TwitterBot.AzureFunctions.Common
{
    public static class Constants
    {
        public const string TWITTER_API_KEY = "TwitterApiKey";
        public const string TWITTER_API_SECRET = "TwitterApiSecret";

        public const string COSMOS_DB_API_KEY = "DatabaseAuthKey";
        public const string COSMOS_DB_DATABASE_ID = "DatabaseId";
        public const string COSMOS_DB_ENDPOINT_URI = "DatabaseEndpointUri";

        public const string SERVICE_BUS_CONNECTION_STRING = "TwitterBotServiceBus_ConnectionString";
        public const string SERVICE_BUS_QUEUE_NAME = "TwitterBotServiceBus_QueueName";
    }
}

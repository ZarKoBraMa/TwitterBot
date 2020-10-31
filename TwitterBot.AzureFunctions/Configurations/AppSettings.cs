namespace TwitterBot.AzureFunctions.Configurations
{
    public class AppSettings
    {
        public string DatabaseId { get; set; }
        public string TwitterBotServiceBus_QueueName { get; set; }
        public int TweetsFilterIntervalInDays { get; set; }
        public int HashtagSyncIntervalInMinutes { get; set; }
        public int HashtagQueueThresholdIntervalInHours { get; set; }
    }
}
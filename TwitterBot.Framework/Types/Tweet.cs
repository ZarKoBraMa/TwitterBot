namespace TwitterBot.Framework.Types
{
    public class Tweet : BaseType
    {
        public string FullText { get; set; }
        public int RetweetCount { get; set; }
        public int FavoriteCount { get; set; }
    }
}

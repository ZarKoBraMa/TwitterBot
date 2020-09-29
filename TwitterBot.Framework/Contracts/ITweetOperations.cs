using TwitterBot.Framework.Types;

namespace TwitterBot.Framework.Contracts
{
    public interface ITweetOperations
    {
        Tweet GetPopularTweetByHashtag(Hashtag hashtag);
    }
}

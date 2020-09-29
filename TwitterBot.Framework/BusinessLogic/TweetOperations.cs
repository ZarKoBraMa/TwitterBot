using AutoMapper;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using TwitterBot.Framework.Contracts;
using TwitterBot.Framework.Types;

namespace TwitterBot.Framework.BusinessLogic
{
    public class TweetOperations : ITweetOperations
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly IMapper _mapper;

        public TweetOperations(TwitterApiSettings twitterApiSettings, IMapper mapper)
        {
            _consumerKey = twitterApiSettings.Key;
            _consumerSecret = twitterApiSettings.Secret;
            _mapper = mapper;
        }

        public Types.Tweet GetPopularTweetByHashtag(Hashtag hashtag)
        {
            Auth.SetApplicationOnlyCredentials(_consumerKey, _consumerSecret, true);

            var searchParameter = Search.CreateTweetSearchParameter(hashtag.Text);
            searchParameter.SearchType = SearchResultType.Popular;
            searchParameter.MaximumNumberOfResults = 1;

            var tweets = Search.SearchTweets(searchParameter);

            if (!tweets.Any())
            {
                return null;
            }

            var topTweet = tweets.FirstOrDefault();

            return _mapper.Map<Types.Tweet>(topTweet);
        }
    }
}

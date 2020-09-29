using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public TweetOperations(string consumerKey, string consumerSecret, IMapper mapper)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
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

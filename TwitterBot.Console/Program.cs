using AutoMapper;
using System;
using System.Resources;
using TwitterBot.Framework.BusinessLogic;
using TwitterBot.Framework.Mappings;

namespace TwitterBot.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var consumerApi = "BLl8j529yRK4i9VtOJCleu8nd";
            var consumerSecret = "JlPQNRANKycrCSXZrPvRyBeDi6zudZKARJwA0SxQ2YKTLDi1Yd";

            var mapperConfiguration = new MapperConfiguration(config => config.AddProfile<MappingProfile>());
            var mapper = mapperConfiguration.CreateMapper();

            var tweetOperations = new TweetOperations(consumerApi, consumerSecret, mapper);
            var result = tweetOperations.GetPopularTweetByHashtag(new Framework.Types.Hashtag
            {
                Text = "#asp.net"
            });

            if (result != null)
            {
                Console.WriteLine(result.FullText);
            }

            Console.ReadKey();
        }
    }
}

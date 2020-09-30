using AutoMapper;
using System;
using System.Resources;
using System.Threading.Tasks;
using TwitterBot.Framework.BusinessLogic;
using TwitterBot.Framework.CosmosDB;
using TwitterBot.Framework.Mappings;
using TwitterBot.Framework.Types;

namespace TwitterBot.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = InternalHandler();
            t.Wait();
            Console.ReadLine();
        }

        private static async Task InternalHandler()
        {
            var context = new DocumentDbContext()
            {
                AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                DatabaseId = "TestDB",
                EndpointUri = "https://localhost:8081"
            };

            await context.CreateDatabaseAndCollectionsAsync();
            var documentRepo = new DocumentDbRepository<Tweet>(context);

            var id1 = Guid.NewGuid().ToString();
            var id2 = Guid.NewGuid().ToString();

            // Create Tweets
            var Tweet1 = await documentRepo.AddOrUpdateAsync(new Tweet
            {
                Id = id1,
                FullText = "This is Test!!!"
            });
            Console.WriteLine("===== Create =====");
            Console.WriteLine(Tweet1.FullText);
            Console.WriteLine("==================");
            var Tweet2 = await documentRepo.AddOrUpdateAsync(new Tweet
            {
                Id = id2,
                FullText = "This is second test!!!"
            });
            Console.WriteLine("===== Create =====");
            Console.WriteLine(Tweet2.FullText);
            Console.WriteLine("==================");

            // Update Tweet
            Tweet2 = await documentRepo.AddOrUpdateAsync(new Tweet
            {
                Id = id2,
                FullText = "This is 2nd test!!!"
            });
            Console.WriteLine("===== Update =====");
            Console.WriteLine(Tweet2.FullText);
            Console.WriteLine("==================");

            // Get By Id Tweet
            Tweet2 = await documentRepo.GetByIdAsync(id2);
            Console.WriteLine("===== GetByIdAsync =====");
            Console.WriteLine(Tweet2.FullText);
            Console.WriteLine("==================");

            // Where
            var tweets = await documentRepo.WhereAsync(p => p.FullText.Contains("e"));
            Console.WriteLine("===== Where =====");
            foreach (var Tweet in tweets)
            {
                Console.WriteLine(Tweet.FullText);
            }
            Console.WriteLine("==================");

            // Top
            var topTweets = await documentRepo.TopAsync(p => p.FullText.
            Contains("e"), 1);
            Console.WriteLine("===== Top =====");
            foreach (var Tweet in topTweets)
            {
                Console.WriteLine(Tweet.FullText);
            }
            Console.WriteLine("==================");
        }
    }
}

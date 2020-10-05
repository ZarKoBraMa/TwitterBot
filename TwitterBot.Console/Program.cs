using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitterBot.Framework.BusinessLogic;
using TwitterBot.Framework.CosmosDB;
using TwitterBot.Framework.Mappings;
using TwitterBot.Framework.ServiceBus;
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
            var serviceBusContext = new ServiceBusContext()
            {
                ConnectionString = "Endpoint=sb://zarkoba.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xfjIl3a0GTGZ5SYBbi3Tptu8PKeMkZiSL5O5vIrchis=",
                QueueName = "HashTagQueue",
                MaxConcurrentMessagesToBeRetrieved = 2,
                SessionId = "TwitterBotApplication",
                OperationTimeout = TimeSpan.FromMilliseconds(500)
            };

            var serviceBusOperations = new ServiceBusOperations(serviceBusContext);
            
            // Create Hashtags
            List<Hashtag> hashtags = new List<Hashtag>();
            for (int i = 1; i <= 10; i++)
            {
                hashtags.Add(new Hashtag
                {
                    Id = $"{i}",
                    Text = $"#justsaying{i}",
                    IsCurrentlyInQueue = true
                });
            }

            // Send messages.
            foreach (var hashtag in hashtags)
            {
                await serviceBusOperations.SendMessageAsync(hashtag.Id, JsonConvert.SerializeObject(hashtag));
            }

            // Receive all Session based messages.
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                var messages = await serviceBusOperations.ReceiveMessagesAsync();
                if (messages != null && messages.Any())
                {
                    foreach (var message in messages)
                    {
                        var messageText = Encoding.UTF8.GetString(message.Body);
                        var hashTag = JsonConvert.DeserializeObject<Hashtag>(messageText);
                        Console.WriteLine(hashTag.Text);
                    }
                }
                else
                {
                    Console.WriteLine("No messages received!!!");
                }
            }
        }
    }
}

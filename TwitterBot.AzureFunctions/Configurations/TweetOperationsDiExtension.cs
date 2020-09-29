using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TwitterBot.AzureFunctions.Common;
using TwitterBot.Framework.BusinessLogic;
using TwitterBot.Framework.Contracts;
using TwitterBot.Framework.Mappings;
using TwitterBot.Framework.Types;

namespace TwitterBot.AzureFunctions.Configurations
{
    public static class TweetOperationsDiExtension
    {
        public static IServiceCollection AddTweetOperations(this IServiceCollection services, IConfiguration configuration)
        {
            var twitterApiSettings = new TwitterApiSettings
            {
                Key = configuration[Constants.TWITTER_API_KEY],
                Secret = configuration[Constants.TWITTER_API_SECRET]
            };

            services
                .AddSingleton(twitterApiSettings)                
                .AddSingleton<ITweetOperations, TweetOperations>();

            return services;
        }
    }
}

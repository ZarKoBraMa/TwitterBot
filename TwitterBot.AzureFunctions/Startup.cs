using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TwitterBot.AzureFunctions.Common;
using TwitterBot.Framework.BusinessLogic;
using TwitterBot.Framework.Contracts;
using TwitterBot.Framework.Mappings;
using TwitterBot.Framework.Types;

[assembly: FunctionsStartup(typeof(TwitterBot.AzureFunctions.Startup))]

namespace TwitterBot.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add IConfiguration
            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureRoot = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";

            var actualRoot = localRoot ?? azureRoot;

            var config = new ConfigurationBuilder()
                .SetBasePath(actualRoot)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton(config);
            _configuration = config;

            var twitterApiSettings = new TwitterApiSettings
            {
                Key = _configuration[Constants.TWITTER_API_KEY],
                Secret = _configuration[Constants.TWITTER_API_SECRET]
            };
            builder.Services.AddSingleton(twitterApiSettings);

            builder.Services.AddSingleton(factory =>
            {
                var mapperConfiguration = new MapperConfiguration(config => config.AddProfile<MappingProfile>());
                return mapperConfiguration.CreateMapper();
            });

            builder.Services.AddSingleton<ITweetOperations, TweetOperations>();
        }
    }
}

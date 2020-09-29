using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwitterBot.AzureFunctions.Common;
using TwitterBot.Framework.BusinessLogic;
using TwitterBot.Framework.Contracts;
using TwitterBot.Framework.Mappings;

[assembly: FunctionsStartup(typeof(TwitterBot.AzureFunctions.Startup))]

namespace TwitterBot.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add IConfiguration
            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureRoot = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";

            var actualRoot = localRoot ?? azureRoot;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(actualRoot)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configBuilder.Build();
            builder.Services.AddSingleton(configuration);

            // Add custom service
            builder.Services.AddSingleton<ITweetOperations>(factory => 
            {
                var consumerApi = configuration[Constants.TWITTER_API_KEY];
                var consumerSecret = configuration[Constants.TWITTER_API_SECRET];

                var mapperConfiguration = new MapperConfiguration(config => config.AddProfile<MappingProfile>());
                var mapper = mapperConfiguration.CreateMapper();

                return new TweetOperations(consumerApi, consumerSecret, mapper);
            });
        }
    }
}

using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TwitterBot.AzureFunctions.Common;
using TwitterBot.AzureFunctions.Configurations;
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
            _configuration = BuildConfiguration();

            
            builder.Services
                .AddSingleton(_configuration)
                .AddAutoMapper()
                .AddTweetOperations(_configuration);
        }

        private IConfiguration BuildConfiguration()
        {   
            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureRoot = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";

            var actualRoot = localRoot ?? azureRoot;

            return new ConfigurationBuilder()
                .SetBasePath(actualRoot)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}

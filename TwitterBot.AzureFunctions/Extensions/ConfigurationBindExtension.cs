using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitterBot.AzureFunctions.Configurations;

namespace TwitterBot.AzureFunctions.Extensions
{
    public static class ConfigurationBindExtension
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration) 
            => services
                .AddSingleton(configuration)
                .Configure<AppSettingsConfiguration>(configuration);
    }
}

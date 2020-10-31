using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TwitterBot.Framework.Mappings;

namespace TwitterBot.AzureFunctions.Extensions
{
    public static class AutoMapperDiExtension
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            return services.AddSingleton(factory =>
            {
                var mapperConfiguration = new MapperConfiguration(config => config.AddProfile<MappingProfile>());
                return mapperConfiguration.CreateMapper();
            });
        }
    }
}

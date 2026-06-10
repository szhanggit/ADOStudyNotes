using Domain.Models.ConfigOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceMedia.Api.Extensions
{
    public static class StoragePathExtension
    {
        public static IServiceCollection AddStoragePath(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StoragePathConfiguration>(options => configuration.GetSection("StoragePathConfiguration").Bind(options));
            return services;
        }
    }
}

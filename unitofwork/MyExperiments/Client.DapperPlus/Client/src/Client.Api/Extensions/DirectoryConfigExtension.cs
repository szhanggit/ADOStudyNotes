using Domain.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Api.Extensions
{
    public static class DirectoryConfigExtension
    {
        public static IServiceCollection ConfigureDirectoryConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DirectoryConfig>(options => configuration.GetSection("DirectoryConfig").Bind(options));
            return services;
        }
    }
}

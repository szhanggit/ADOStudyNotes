using Domain.Models.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Service.Extensions
{
    [ExcludeFromCodeCoverageAttribute]
    public static class DirectoryConfigExtension
    {
        public static IServiceCollection ConfigureDirectoryConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DirectoryConfig>(options => configuration.GetSection("DirectoryConfig").Bind(options));
            return services;
        }
    }
}

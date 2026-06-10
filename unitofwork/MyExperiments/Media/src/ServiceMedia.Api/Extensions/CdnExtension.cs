using Domain.Models.ConfigOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceMedia.Api.Extensions
{
    public static class CdnExtension
    {
        public static IServiceCollection AddCdn(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CdnConfiguration>(options => configuration.GetSection("CdnConfiguration").Bind(options));
            return services;
        }
    }
}

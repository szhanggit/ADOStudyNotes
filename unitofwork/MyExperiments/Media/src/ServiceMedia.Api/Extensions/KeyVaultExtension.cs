using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TXC.Common.Services.KeyVault;

namespace ServiceMedia.Api.Extensions
{
    public static class KeyVaultExtension
    {
        public static IServiceCollection AddKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IKeyVaultServices, KeyVaultService>();
            return services;
        }
    }
}

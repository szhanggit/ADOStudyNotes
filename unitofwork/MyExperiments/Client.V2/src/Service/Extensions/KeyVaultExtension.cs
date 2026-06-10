using TXC.Common.Services.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Extensions
{
    public static class KeyVaultExtension
    {
        public static IServiceCollection ConfigureKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IKeyVaultServices, KeyVaultService>();
            return services;
        }
    }
}

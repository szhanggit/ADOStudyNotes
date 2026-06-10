using TXC.Common.Services.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Service.Extensions
{
    [ExcludeFromCodeCoverageAttribute]
    public static class KeyVaultExtension
    {
        public static IServiceCollection ConfigureKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IKeyVaultServices, KeyVaultService>();
            return services;
        }
    }
}

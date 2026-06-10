using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services.Storage.Config;

namespace Service.Extensions
{
    [ExcludeFromCodeCoverageAttribute]
    public static class AzureStorageExtension
    {
        public static IServiceCollection ConfigureAzureStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageConfig>(options => configuration.GetSection("AzureStorageConfiguration").Bind(options));
            return services;
        }
    }
}

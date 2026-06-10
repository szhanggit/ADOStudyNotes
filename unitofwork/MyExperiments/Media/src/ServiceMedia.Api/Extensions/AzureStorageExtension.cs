using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TXC.Common.Services.Storage;
using TXC.Common.Services.Storage.Config;

namespace ServiceMedia.Api.Extensions
{
    public static class AzureStorageExtension
    {
        public static IServiceCollection AddAzureStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageConfig>(options => configuration.GetSection("AzureStorageConfiguration").Bind(options));
            services.AddTransient<IAzureBlobService, AzureBlobService>();
            return services;
        }
    }
}

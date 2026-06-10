using Domain.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Api.Extensions
{
    public static class AzureStorageExtension
    {
        public static IServiceCollection ConfigureAzureStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageConfig>(options => configuration.GetSection("AzureStorageConfiguration").Bind(options));
            //services.AddTransient<IAzureBlobService, AzureBlobService>();
            return services;
        }
    }
}

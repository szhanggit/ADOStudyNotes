using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TXC.Common.CacheManagement.Extensions;
using TXC.Common.CacheManagement.Interface;
using TXC.Common.CacheManagement.Operation;
using TXC.Common.CacheManagement.ProgramCollection;
using TXC.Common.CacheManagement.Resolver;

namespace ServiceMedia.Api.Extensions
{
    public static class CachedExtension
    {
        public static IServiceCollection AddCached(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            //todo: temporary use local only
            services.ConfigureTxcDistributedCache(true, configuration);
            services.ConfigureTxcGrpcDistributedCache(true, configuration);

            services.AddSingleton<ICacheOperation, CacheOperation>();
            services.AddSingleton<TenantConfigCacheRead>();
            services.AddSingleton<ITxcCacheReadFactory, TxcCacheReadFactoryGrpc>();

            return services;
        }
    }
}

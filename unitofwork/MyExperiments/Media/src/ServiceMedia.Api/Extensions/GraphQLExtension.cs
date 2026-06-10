using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Utility.GraphQLClient;

namespace Api.Extensions
{
    public static class GraphQLExtension
    {
        public static void AddGraphQLExtService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITXCGraphQLClientResolver, TXCGraphQLClientResolver>();
            services.AddScoped<ITXCGraphQLClientResolverWithHeader, TXCGraphQLClientResolverWithHeader>();
            services.AddScoped<DictionaryGraphQLClient>();
            services.AddScoped<TPCGraphQLClient>();
            services.AddScoped<MerchantGraphQLClient>();
            services.AddScoped<GatewayGraphQLClient>();
            services.AddScoped<IMediaGraphQLClient,MediaGrapQLClient>();
        }
    }
}

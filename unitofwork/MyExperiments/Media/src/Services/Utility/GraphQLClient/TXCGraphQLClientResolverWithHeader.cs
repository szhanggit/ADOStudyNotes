using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Services.Utility.GraphQLClient
{
    public interface ITXCGraphQLClientResolverWithHeader
    {
        public ITXCGraphqlClientWithHeader GetGraphQLClient(ETXCGraphQLClient instance);
    }
    [ExcludeFromCodeCoverageAttribute]
    public class TXCGraphQLClientResolverWithHeader : ITXCGraphQLClientResolverWithHeader
    {
        private readonly IServiceProvider _provider;

        public TXCGraphQLClientResolverWithHeader(IServiceProvider provider)
        {
            this._provider = provider;
        }
        /// <summary>
        /// get graphql set up based on ehy resolver
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public ITXCGraphqlClientWithHeader GetGraphQLClient(ETXCGraphQLClient instance)
        {
            switch (instance)
            {
                case ETXCGraphQLClient.Gateway:
                    return this._provider.GetService<GatewayGraphQLClient>();
                case ETXCGraphQLClient.Media:
                    return this._provider.GetService<MediaGrapQLClient>();
            }

            return null;
        }
    }
}

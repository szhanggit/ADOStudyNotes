using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Services.Utility.GraphQLClient
{
    public enum ETXCGraphQLClient
    {
        Dictionary,
        TPC,
        Gateway,
        Merchant,
        Media
    }
    public interface ITXCGraphQLClientResolver
    {
        public ITXCGraphQLClient GetGraphQLClient(ETXCGraphQLClient instance);
    }
    [ExcludeFromCodeCoverageAttribute]
    public class TXCGraphQLClientResolver : ITXCGraphQLClientResolver
    {
        private readonly IServiceProvider _provider;

        public TXCGraphQLClientResolver(IServiceProvider provider)
        {
            this._provider = provider;
        }
        public ITXCGraphQLClient GetGraphQLClient(ETXCGraphQLClient instance)
        {
            switch (instance)
            {
                case ETXCGraphQLClient.Merchant:
                    return this._provider.GetService<MerchantGraphQLClient>();
                case ETXCGraphQLClient.Dictionary:
                    return this._provider.GetService<DictionaryGraphQLClient>();
                case ETXCGraphQLClient.TPC:
                    return this._provider.GetService<TPCGraphQLClient>();
            }
            return null;
        }
    }
}

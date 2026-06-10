using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Utility.GraphQLClient
{
    [ExcludeFromCodeCoverageAttribute]
    public class MerchantGraphQLClient : ITXCGraphQLClient
    {
        public IGraphQLClient GraphQLClient { get; }
        public MerchantGraphQLClient(IConfiguration configuration)
        {
            GraphQLClient = new GraphQLHttpClient(configuration["ServiceUrlConfiguration:ServiceMerchantGraphQLUrl"], new NewtonsoftJsonSerializer());
        }

        public void AddHeaders(string key, string value)
        {
            (GraphQLClient as GraphQLHttpClient).HttpClient.DefaultRequestHeaders.Add(key, value);
        }
    }
}

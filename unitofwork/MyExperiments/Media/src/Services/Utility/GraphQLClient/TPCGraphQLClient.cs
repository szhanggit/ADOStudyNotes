using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Services.Utility.GraphQLClient
{
    [ExcludeFromCodeCoverageAttribute]
    public class TPCGraphQLClient : ITXCGraphQLClient
    {
        public IGraphQLClient GraphQLClient { get; }
        public TPCGraphQLClient(IConfiguration configuration)
        {
            GraphQLClient = new GraphQLHttpClient(configuration["ServiceUrlConfiguration:ServiceTpcVendorUrl"], new NewtonsoftJsonSerializer());
        }
        public void AddHeaders(string key, string value)
        {
            if (!(GraphQLClient as GraphQLHttpClient).HttpClient.DefaultRequestHeaders.Contains(key))
                (GraphQLClient as GraphQLHttpClient).HttpClient.DefaultRequestHeaders.Add(key, value);
        }
    }
}

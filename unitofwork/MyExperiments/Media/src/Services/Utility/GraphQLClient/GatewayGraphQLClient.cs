using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Services.Utility.GraphQLClient
{
    [ExcludeFromCodeCoverageAttribute]
    public class GatewayGraphQLClient : ITXCGraphqlClientWithHeader
    {
        private readonly IConfiguration _configuration;
        public GatewayGraphQLClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private GraphQLHttpClient _graphQLHttpClient { get; set; }

        public IGraphQLClient GetGraphQLClient(int tenantId)
        {
            if (_graphQLHttpClient == null)
            {
                _graphQLHttpClient = new GraphQLHttpClient(_configuration["ServiceUrlConfiguration:ServiceGraphQLGatewayUrl"], new NewtonsoftJsonSerializer());
                _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("TenantId", tenantId.ToString());
            }


            return _graphQLHttpClient;
        }
    }
}

using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Service.Utility.GraphQLClient.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Service.Utility.GraphQLClient
{
    public interface IGraphQLGatewayClient : ITXCGraphqlClientWithHeader
    {
        IGraphQLClient GetGraphQLClient(int tenantId);
    }

    [ExcludeFromCodeCoverageAttribute]
    public class GraphQLGatewayClient : IGraphQLGatewayClient
    {
        private readonly IConfiguration _configuration;
        public GraphQLGatewayClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private GraphQLHttpClient _graphQLHttpClient { get; set; }

        public IGraphQLClient GetGraphQLClient(int tenantId)
        {
            if (_graphQLHttpClient == null)
            {
                var svcClient = _configuration["ServiceUrlConfiguration:ServiceGraphQLGatewayUrl"];
                if (!String.IsNullOrWhiteSpace(svcClient))
                {
                    _graphQLHttpClient = new GraphQLHttpClient(_configuration["ServiceUrlConfiguration:ServiceGraphQLGatewayUrl"], new NewtonsoftJsonSerializer());
                    _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("TenantId", tenantId.ToString());
                }
            }


            return _graphQLHttpClient;
        }
    }
}

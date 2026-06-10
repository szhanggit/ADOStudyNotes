using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using Service.Utility.GraphQLClient.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Service.Utility.GraphQLClient
{
    public interface IClientGraphQLClient : ITXCGraphqlClientWithHeader
    {
    }

    [ExcludeFromCodeCoverageAttribute]
    public class ClientGraphQLClient : IClientGraphQLClient
    {
        private readonly IConfiguration _configuration;
        public ClientGraphQLClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private GraphQLHttpClient _graphQLHttpClient { get; set; }

        public IGraphQLClient GetGraphQLClient(int tenantId)
        {
            if (_graphQLHttpClient == null)
            {
                var svcClient = _configuration["ServiceUrlConfiguration:ServiceClientGrapQLUrl"];
                if (!String.IsNullOrWhiteSpace(svcClient))
                {
                    _graphQLHttpClient = new GraphQLHttpClient(_configuration["ServiceUrlConfiguration:ServiceClientGrapQLUrl"], new NewtonsoftJsonSerializer());
                    _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("TenantId", tenantId.ToString());
                }
            }


            return _graphQLHttpClient;
        }
    }
}

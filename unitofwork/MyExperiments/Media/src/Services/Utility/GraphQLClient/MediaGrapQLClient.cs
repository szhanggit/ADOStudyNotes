using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Utility.GraphQLClient
{

    public interface IMediaGraphQLClient: ITXCGraphqlClientWithHeader
    {

    }
    public class MediaGrapQLClient : IMediaGraphQLClient
    {
        private readonly IConfiguration _configuration;
        public MediaGrapQLClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private GraphQLHttpClient _graphQLHttpClient { get; set; }

        public IGraphQLClient GetGraphQLClient(int tenantId)
        {
            if (_graphQLHttpClient == null)
            {
                var svcMedia = _configuration["ServiceUrlConfiguration:ServiceMediaGrapQLUrl"];
                if (!String.IsNullOrWhiteSpace(svcMedia))
                {
                    _graphQLHttpClient = new GraphQLHttpClient(_configuration["ServiceUrlConfiguration:ServiceMediaGrapQLUrl"], new NewtonsoftJsonSerializer());
                    _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("TenantId", tenantId.ToString());
                }
            }


            return _graphQLHttpClient;
        }
    }
}

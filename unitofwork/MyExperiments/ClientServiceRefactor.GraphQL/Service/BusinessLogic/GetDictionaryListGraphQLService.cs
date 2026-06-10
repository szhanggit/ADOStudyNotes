using Domain.Models;
using Domain.Models.Response;
using GraphQL;
using Service.Utility.GraphQLClient;
using System.Text;

namespace Service.BusinessLogic
{
    public interface IGetDictionaryListGraphQLService
    {
        Task<List<ProvinceCityPairModel>> GetProvinceCityPairListAsync(int TenantId, int CountryId);
    }
    public class GetDictionaryListGraphQLService : IGetDictionaryListGraphQLService
    {
        private IGeneralGraphQLClient _graphQLClient;
        public GetDictionaryListGraphQLService(IGeneralGraphQLClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
        }

        public async Task<List<ProvinceCityPairModel>> GetProvinceCityPairListAsync(int TenantId, int CountryId)
        {
            List<ProvinceCityPairModel> _getProvinceCityPairList = new List<ProvinceCityPairModel>();
            var graphqlClient = _graphQLClient.GetGraphQLClient(TenantId);
			StringBuilder _querySB = new StringBuilder();
			_querySB.AppendLine("query{");
			_querySB.AppendLine($"provinceCityPairList(countryId: {CountryId})");
			_querySB.AppendLine(@"{items {
			                        city,
			                        province	                          
                              }
                            }
                        }");

            var query = new GraphQLRequest
            {
                Query = _querySB.ToString(),
            };

            if (graphqlClient != null)
            {
                GraphQLResponse<GetProvinceCityPairListGraphQLResponse> clientFetchingResult = await graphqlClient.SendQueryAsync<GetProvinceCityPairListGraphQLResponse>(query, default);
                if (clientFetchingResult.Data != null)
                {
                    _getProvinceCityPairList.AddRange(clientFetchingResult.Data.provinceCityPairList.Items);
                }
            }

            return _getProvinceCityPairList;
        }
    }
}

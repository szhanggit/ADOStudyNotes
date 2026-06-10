using Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Service.BusinessLogic;
using Service.Utility;
using Service.Utility.GraphQLClient;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Test
{
    public class GetDictionaryListGraphQLServiceTest
    {
        private IGeneralGraphQLClient _graphQLClient = null;
        private IGetDictionaryListGraphQLService _getDictionaryListGraphQLService = null;

        public GetDictionaryListGraphQLServiceTest()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            _graphQLClient = new GeneralGraphQLClient(config);
            _getDictionaryListGraphQLService = new GetDictionaryListGraphQLService(_graphQLClient);
        }

        [Fact]
        public async Task TestGetProvinceCityPairListAsync()
        {
            List<ProvinceCityPairModel> _list = await _getDictionaryListGraphQLService.GetProvinceCityPairListAsync(9, 6);
        }
    }
}
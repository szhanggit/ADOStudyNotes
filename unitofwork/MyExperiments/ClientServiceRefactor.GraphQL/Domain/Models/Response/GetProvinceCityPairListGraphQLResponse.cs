using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetProvinceCityPairListGraphQLResponse
    {
        public GetProvinceCityPairListGraphQLResponseItem provinceCityPairList { get; set; }
    }

    [ExcludeFromCodeCoverageAttribute]
    public class GetProvinceCityPairListGraphQLResponseItem
    { 
        public List<ProvinceCityPairModel> Items { get; set; }
    }
}

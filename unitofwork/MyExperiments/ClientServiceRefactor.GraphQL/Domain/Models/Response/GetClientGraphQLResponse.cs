using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetClientGraphQLResponse
    {
        public ClientGraphQLResponseItem Client { get; set; }
        public ClientGraphQLResponseItem ClientBySearchKey { get; set; }
    }

    [ExcludeFromCodeCoverageAttribute]
    public class ClientGraphQLResponseItem
    {
        public List<ClientModel> Items { get; set; }
        public int TotalCount { get; set; }
    }
}

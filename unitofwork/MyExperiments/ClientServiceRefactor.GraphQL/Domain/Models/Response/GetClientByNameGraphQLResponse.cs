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
    public class GetClientByNameGraphQLResponse
    {
        public List<Client> ClientByName { get; set; }
    }
}

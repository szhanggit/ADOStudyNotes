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
    public class GetClientByIdGraphQLResponse
    {
        public List<ClientModel> ClientByID { get; set; }
    }
}

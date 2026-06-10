using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e = Entities;

namespace Services.GraphQLResponse
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetAllMediaGraphQLResponse
    {
        public MediaGraphQLResponseItem media { get; set; }
    }

    [ExcludeFromCodeCoverageAttribute]
    public class MediaGraphQLResponseItem
    {
        public List<e.Media> Items { get; set; }
    }
}

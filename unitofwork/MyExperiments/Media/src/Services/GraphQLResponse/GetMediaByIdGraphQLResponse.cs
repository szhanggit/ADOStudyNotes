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
    public class GetMediaByIdGraphQLResponse
    {
        public List<e.Media> mediaById { get; set; }
        public e.Media Media => mediaById.FirstOrDefault();
    }
}

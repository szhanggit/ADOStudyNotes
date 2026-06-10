using Domain.EnumList;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetAnyMediaNameTypeQuery : IRequestWrapper<bool>
    {
        public string Keyword { get; set; }
        public ImageCategory Type { get; set; }
    }
}

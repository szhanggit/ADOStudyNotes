using Domain.Dto;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetMediaQuery : IRequestWrapper<MediaDto>
    {
        public int MediaId { get; set; }
    }
}

using Domain.Dto;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetMediaNameQuery : IRequestWrapper<MediaDto>
    {
        public string BlobName { get; set; }
        public int MediaId { get; set; }
    }
}

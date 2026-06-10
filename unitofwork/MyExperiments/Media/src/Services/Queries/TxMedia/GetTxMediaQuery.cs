using Domain.Dto;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.TxMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetTxMediaQuery : IRequestWrapper<TxMediaDto>
    {
        public int MediaId { get; set; }
    }
}

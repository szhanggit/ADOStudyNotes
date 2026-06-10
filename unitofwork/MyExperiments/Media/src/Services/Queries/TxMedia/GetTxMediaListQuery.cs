using Domain.Dto;
using Domain.EnumList;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.TxMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetTxMediaListQuery : IRequestWrapper<IEnumerable<TxMediaDto>>
    {
        public string SearchKey { get; set; }
        public ImageCategory MediaCategory { get; set; }
    }
}

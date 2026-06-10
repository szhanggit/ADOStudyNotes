using Domain.DTOs;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Domain.Models.Pagination;

namespace Domain.Models.Response
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetClientListResponse : IPaginationTotal
    {
        public IList<ClientDto> ClientDtos { get; set; }
        public int TotalCount { get; set; }
    }
}

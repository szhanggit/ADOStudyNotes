using Domain.Dto;
using System.Collections.Generic;
using TXC.Common.Domain.Models.Pagination;

namespace Domain.Models.Response
{
    public class GetClientListResponse : IPaginationTotal
    {
        public IList<ClientDto> ClientDtos { get; set; }
        public int TotalCount { get; set; }
    }
}

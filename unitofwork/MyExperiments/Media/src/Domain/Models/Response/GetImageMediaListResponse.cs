using Domain.Dto;
using System.Collections.Generic;
using TXC.Common.Domain.Models.Pagination;

namespace Domain.Models.Response
{
    public class GetImageMediaListResponse : IPaginationTotal
    {
        public IList<MediaDto> MediaDtos { get; set; }
        public int TotalCount { get ; set; }
    }
}

using Domain.Models.Response;
using TXC.Common.Domain.Models.Pagination;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.Client
{
    public class GetClientListQuery : Pagination, IRequestListWrapper<GetClientListResponse>
    {
        public string SearchKeyword { get; set; }
        public int ClientId { get; set; }
    }
}

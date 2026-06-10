using Domain.EnumList;
using Domain.Models.Response;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Domain.Models.Pagination;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetImageMediaListQuery : Pagination, IRequestListWrapper<GetImageMediaListResponse>
    {
        public string SearchKeyword { get; set; }
        public ImageCategory Type {get;set;}
    }
}

using Domain.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.User
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetAllSampleQueryHandler : IRequestHandlerWrapper<GetAllSampleQuery, IEnumerable<SampleInfoModel>>
    {



        public async Task<Response<IEnumerable<SampleInfoModel>>> Handle(GetAllSampleQuery request, CancellationToken cancellationToken)
        {

            var data = new List<SampleInfoModel>()
            {
                new SampleInfoModel()
                {
                    UserInfoId = 1,
                    UserId = "homerio",
                    UserName = $"homerio {request.UserId}",
                    Email = "h.r.sumabat@gmail.com"
                },
                new SampleInfoModel()
                {
                    UserInfoId = 2,
                    UserId = "emsi",
                    UserName = "emsi_homer",
                    Email = "emsi@gmail.com"
                }
            };

            var result = await Task.FromResult(Response.Success<IEnumerable<SampleInfoModel>>("Successfully retrieve", data));
            return result;
        }
    }
}

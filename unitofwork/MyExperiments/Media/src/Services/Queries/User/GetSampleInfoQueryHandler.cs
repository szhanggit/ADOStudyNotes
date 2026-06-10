using Domain.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.User
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetSampleInfoQueryHandler : IRequestHandlerWrapper<GetSampleInfoQuery, SampleInfoModel>
    {
        private readonly GetAllSampleQueryHandler getAllUsersQueryHandler;
        public GetSampleInfoQueryHandler()
        {
            getAllUsersQueryHandler = new GetAllSampleQueryHandler();
        }
        public async Task<Response<SampleInfoModel>> Handle(GetSampleInfoQuery request, CancellationToken cancellationToken)
        {
            var allUsers = (await getAllUsersQueryHandler.Handle(new GetAllSampleQuery(), cancellationToken)).Data.ToList();

            var result = allUsers.FirstOrDefault(f => f.UserId == request.UserId);
            if (result != null)
            {
                return Response.Success("Successfully retrieve", result);
            }
            else
            {
                return Response.Fail("Failed to retrieve", new SampleInfoModel());
            }
        }
    }
}

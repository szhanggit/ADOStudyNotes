using Domain.Models;
using System.Diagnostics.CodeAnalysis;
using TXC.Common.Services;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.User
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetSampleInfoQuery : BaseRequest, IRequestWrapper<SampleInfoModel>
    {
    }
}

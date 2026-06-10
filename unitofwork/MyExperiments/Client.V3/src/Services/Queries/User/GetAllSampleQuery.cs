using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.Services;
using TXC.Common.Services.Wrappers;

namespace Services.Queries.User
{
    public class GetAllSampleQuery : BaseRequest, IRequestWrapper<IEnumerable<SampleInfoModel>>
    {
    }
}

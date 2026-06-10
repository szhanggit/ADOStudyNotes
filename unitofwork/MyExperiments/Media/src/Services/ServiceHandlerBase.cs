using Domain.Constant;
using Microsoft.AspNetCore.Http;
using TXC.Common.Data.TenantDbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;

namespace Services
{
    public abstract class ServiceHandlerBase
    {
        protected readonly ITenantDbConnection _tenantDbConnection;
        protected readonly IDapperOperation _dapperOperation;

        public ServiceHandlerBase(ITenantDbConnection tenantDbConnection, IDapperOperation dapperOperation)
        {
            _tenantDbConnection = tenantDbConnection;
            _dapperOperation = dapperOperation;
        }

    }
}

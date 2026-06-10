using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Proto.Client;

namespace Infrastructure.Extensions
{
    public static class PaginationExtensions
    {
        public static int GetPageOffset(this GetClientListRequest request)
        {
            return (request.PageNumber >= 1) ? (request.PageNumber - 1) * request.RowCount ?? 0 : request.RowCount ?? 0;
        }
    }
}

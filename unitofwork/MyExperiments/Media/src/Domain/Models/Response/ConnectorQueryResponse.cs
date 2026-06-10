using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response
{
    public class ConnectorQueryResponse<TData>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
    }
}

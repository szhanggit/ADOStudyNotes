using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response
{
    public class ConnectorCommandResponse
    {
        public bool IsSucess { get; set; }
        public string Message { get; set; }
        public int CreatedId { get; set; }
    }
}

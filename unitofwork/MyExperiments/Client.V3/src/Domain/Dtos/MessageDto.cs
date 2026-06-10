using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class MessageDto
    {
        public string Tenant { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
        public object Content { get; set; }
    }
}

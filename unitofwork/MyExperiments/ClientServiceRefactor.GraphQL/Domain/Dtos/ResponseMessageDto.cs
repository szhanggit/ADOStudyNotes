using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    [ExcludeFromCodeCoverageAttribute]
    public class ResponseMessageDto
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }
}

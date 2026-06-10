using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    [ExcludeFromCodeCoverageAttribute]
    public class ConfigDto
    {
        public int TenantId { get; set; }
        public string ConfigType { get; set; }
        public string ConfigName { get; set; }
        public string Version { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
    }
}

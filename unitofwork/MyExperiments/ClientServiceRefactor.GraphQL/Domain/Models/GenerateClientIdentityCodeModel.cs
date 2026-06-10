using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [ExcludeFromCodeCoverageAttribute]
    public class GenerateClientIdentityCodeModel
    {
        public string SequenceName { get; set; }
        public bool IsFixReturnLength { get; set; }
        public byte ReturnLength { get; set; }
        public char PaddingCharacter { get; set; }
        public int TenantId { get; set; }
    }
}

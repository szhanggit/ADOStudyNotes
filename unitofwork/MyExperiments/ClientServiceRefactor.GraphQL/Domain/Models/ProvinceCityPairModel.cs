using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [ExcludeFromCodeCoverageAttribute]
    public class ProvinceCityPairModel
    {
        public int province { get; set; }
        public int city { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Configuration
{
    [ExcludeFromCodeCoverageAttribute]
    public class AzureStorageConfig
    {
        public string ConnectionString { get; set; }
    }
}

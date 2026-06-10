using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TryDapperCoreWebApp1.Models
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string Lvl { get; set; }
    }
}

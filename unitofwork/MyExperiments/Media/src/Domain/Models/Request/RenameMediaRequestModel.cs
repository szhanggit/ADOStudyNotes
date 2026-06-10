using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Request
{
    public class RenameMediaRequestModel
    {
        public int MediaId { get; set; }
        public string KeyWord { get; set; }
    }
}

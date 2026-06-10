using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Request
{
    public class GetAllMediaRequestModel
    {
        public string SearchKey { get; set; }
        public int MediaCategory { get; set; }
    }
}

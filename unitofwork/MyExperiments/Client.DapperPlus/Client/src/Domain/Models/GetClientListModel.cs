using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class GetClientListModel
    {
        public int? RowCount { get; set; }
        public int? PageNumber { get; set; }
        public int? ClientId { get; set; }
        public string SearchKeyWord { get; set; }
    }
}

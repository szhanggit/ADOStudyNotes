using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Request
{
    public class GetMediaNameRequestQueryModel
    {
        public string BlobName { get; set; }
        public int MediaId { get; set; }
    }
}

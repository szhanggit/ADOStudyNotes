using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Request
{
    public class CreateMediaRequestModel
    {
        public int MediaId { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public string NodeUrl { get; set; }
        public string Account { get; set; }
        public string BlobName { get; set; }
        public int Type { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Keyword { get; set; }

    }
}

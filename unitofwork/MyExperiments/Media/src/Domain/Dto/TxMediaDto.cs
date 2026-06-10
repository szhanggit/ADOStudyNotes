using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class TxMediaDto
    {
        public int MediaId { get; set; }
        public string FileName { get; set; }
        public string KeyWord { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Url { get; set; }
        public string BlobName { get; set; }
        public int MediaCategory { get; set; }

    }
}

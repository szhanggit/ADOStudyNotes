using Domain.EnumList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response
{
    public class GetMediaResponseModel
    {
        public int MediaId { get; set; }
        public string FileName { get; set; }
        public string Keyword { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Url { get; set; }
        public string BlobName { get; set; }
        public ImageCategory Type { get; set; }
    }
}

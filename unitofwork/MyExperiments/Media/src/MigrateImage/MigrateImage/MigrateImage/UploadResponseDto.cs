using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateImage
{
    public class MediaUploadResponse
    { 
        public bool IsSuccess { get; set; }
        public BlobMediaInfo Data { get; set; }
    }
    
    public class BlobMediaInfo
    {
        public string AccountName { get; set; }
        public string ContainerName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public string GetFileName()
        {
            if (!string.IsNullOrEmpty(this.Name) && !string.IsNullOrWhiteSpace(this.Name))
            {
                return Path.GetFileName(Name);
            }
            return null;
        }
    }
}

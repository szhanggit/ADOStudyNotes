using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateImage
{
    public class MediaLibrary
    {
        public int Id { get; set; }
        public int MediaCategory { get; set; }
        public string FileName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string KeyWord { get; set; }
        public string PhysicalFullPath { get; set; }
        public string ExternalURL { get; set; }
        
    }
}

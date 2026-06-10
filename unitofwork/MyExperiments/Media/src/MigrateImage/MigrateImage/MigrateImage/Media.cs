using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateImage
{
    [Table("tb_m_media")]
    public class Media
    {
        [ExplicitKey]
        public int media_id { get; set; }
        public string file_name { get; set; }
        public string file_content_type { get; set; }
        public string account { get; set; }
        public string blob_name { get; set; }
        public string node_url { get; set; }
        public int type { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string keyword { get; set; }
    }
}

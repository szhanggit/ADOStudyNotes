using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Entities
{
    [ExcludeFromCodeCoverageAttribute]
    [Table("media.tb_m_media")]
    public class Media
    {
        [Key]
        [JsonProperty("mediaId")]
        public int Media_Id { get; set; }
        [JsonProperty("fileName")]
        public string File_Name { get; set; }
        [JsonProperty("fileContentType")]
        public string File_Content_Type { get; set; }
        [JsonProperty("nodeUrl")]
        public string Node_Url { get; set; }
        public string Account { get; set; }
        [JsonProperty("blobName")]
        public string Blob_Name { get; set; }
        public int Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Keyword { get; set; }
        
    }
}
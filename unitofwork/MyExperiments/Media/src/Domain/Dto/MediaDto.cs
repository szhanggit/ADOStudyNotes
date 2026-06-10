using Domain.EnumList;

namespace Domain.Dto
{
    public class MediaDto
    {
        public int MediaId { get; set; }
        public string FileName { get; set; }
        public string Keyword { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string NodeUrl { get; set; }
        public string BlobName { get; set; }
        public ImageCategory Type { get; set; }
    }
}

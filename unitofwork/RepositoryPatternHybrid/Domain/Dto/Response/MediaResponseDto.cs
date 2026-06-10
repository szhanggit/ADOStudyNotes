using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Response
{
    public class MediaResponseDto
    {
        [Key]
        [Column("media_id")]
        public int MediaId { get; set; }
        [Column("file_name", TypeName = "varchar(100)")]
        [StringLength(100, ErrorMessage = "File name exceeds the maximum limit(100)!")]
        public string FileName { get; set; }
        public string Keyword { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        [Column("node_url", TypeName = "varchar(MAX)")]
        public string NodeUrl { get; set; }
        public string BlobName { get; set; }
        public ImageCategory Type { get; set; }
    }
}

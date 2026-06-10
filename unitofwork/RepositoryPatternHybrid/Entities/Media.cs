using d=Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("tb_m_media", Schema ="media")]
    [d.Table("media.tb_m_media")]
    public class Media
    {
        [Key]
        [d.ExplicitKey]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int media_id { get; set; }
        
        [Column(TypeName = "varchar(100)")]
        [StringLength(100, ErrorMessage ="File name exceeds the maximum limit(100)!")]
        public string? file_name { get; set; }

        [Column(TypeName ="varcha(255)")]
        [StringLength(100, ErrorMessage = "Content type exceeds the maximum limit(255)!")]
        public string? file_content_type { get;set; }

        [Column(TypeName ="varchar(MAX)")]
        public string? node_url { get; set; }

        [Column(TypeName ="varchar(250)")]
        public string? account { get; set; }

        [Column(TypeName ="varchar(250)")]
        public string? blob_name { get; set; }

        [Column(TypeName ="int")]
        [Required(ErrorMessage ="Type is required!")]
        public int type { get; set; }

        [Column(TypeName ="varchar(7)")]
        public string? width { get; set; }

        [Column(TypeName = "varchar(7)")]
        public string? height { get; set; }

        [Column(TypeName = "nvarchar(250)")]      
        public string? keyword { get; set; }


    }
}

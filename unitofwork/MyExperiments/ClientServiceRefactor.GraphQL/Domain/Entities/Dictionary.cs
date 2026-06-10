using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverageAttribute]
    [Table("tb_d_dictionary", Schema = "general")]
    public class Dictionary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("dictionary_id", TypeName = "int")]
        public int DictionaryId { get; set; }
        [Required]
        [Column("category", TypeName = "nvarchar(50)")]
        public string Category { get; set; }
        [Required]
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Required]
        [Column("display_name", TypeName = "nvarchar(500)")]
        public string DisplayName { get; set; }
        [Column("parent_id", TypeName = "int")]
        public int? ParentId { get; set; }
        [NotMapped]
        [Column("timestamp", TypeName = "timestamp")]
        public Byte[] TimeStamp { get; set; }
    }
}

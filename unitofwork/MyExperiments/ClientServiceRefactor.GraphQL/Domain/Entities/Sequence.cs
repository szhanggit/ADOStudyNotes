using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using d = Dapper.Contrib.Extensions;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverageAttribute]
    [d.Table("general.tb_s_sequence")]
    public class Sequence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }
        public int Val { get; set; }
    }
}

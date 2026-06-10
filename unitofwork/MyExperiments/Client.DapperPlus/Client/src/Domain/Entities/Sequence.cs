using System;
using d = Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [d.Table("general.tb_s_sequence")]
    public class Sequence
    {
        [d.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}

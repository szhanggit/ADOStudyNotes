using System;
using d = Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [d.Table("general.tb_d_dictionary")]
    public class Dictionary
    {
        [d.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Dictionary_Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Display_Name { get; set; }
        public int? Parent_Id { get; set; }
        public Byte[] TimeStamp { get; set; }
    }
}

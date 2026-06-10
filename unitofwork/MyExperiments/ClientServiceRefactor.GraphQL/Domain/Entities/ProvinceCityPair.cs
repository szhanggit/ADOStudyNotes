using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverageAttribute]
    [Table("tb_d_dictionary", Schema = "general")]
    public class ProvinceCityPair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int province { get; set; }
        public int city { get; set; }
    }
}

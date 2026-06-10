using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("tb_a_address", Schema = "general")]
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? AddressId { get; set; }
        public string DetailAddressLine { get; set; }
        public string District { get; set; }
        public int? CityId { get; set; }
        public int? StateOrProvinceId { get; set; }
        public string Postcode { get; set; }
        public int? CountryId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? AddressStatus { get; set; }
    }
}

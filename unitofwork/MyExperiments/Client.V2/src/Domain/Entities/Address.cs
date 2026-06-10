using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address
    {
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

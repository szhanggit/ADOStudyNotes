using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models
{
    [ExcludeFromCodeCoverageAttribute]
    public class AddressModel
    {
        public string DetailAddressLine { get; set; }
        public string District { get; set; }
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public string PostCode { get; set; }
        public int? CountryId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int Status { get; set; }
    }
}

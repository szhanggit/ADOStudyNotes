using System;
using d = Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Domain.Entities
{
    [d.Table("general.tb_a_address")]
    public class Address
    {
        [d.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Address_Id { get; set; }
        public string Detail_Address_Line { get; set; }
        public string District { get; set; }
        public int? City_Id { get; set; }
        public int? State_Province_Id { get; set; }
        public string Postcode { get; set; }
        public int? Country_Id { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? Status { get; set; }
        [Write(false)]
        public Byte[] TimeStamp { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using d = Dapper.Contrib.Extensions;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverageAttribute]
    [d.Table("general.tb_a_address")]
    public class Address
    {
        [d.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("address_id", TypeName = "int")]
        public int Address_Id { get; set; }
        [Column("detail_address_line", TypeName = "nvarchar(500)")]
        public string Detail_Address_Line { get; set; }
        [Column("district", TypeName = "nvarchar(100)")]
        public string District { get; set; }
        [Column("city_id", TypeName = "int")]
        public int? City_Id { get; set; }
        [Column("state_province_id", TypeName = "int")]
        public int? State_Province_Id { get; set; }
        [Column("postcode", TypeName = "nvarchar(20)")]
        public string PostCode { get; set; }
        [Column("country_id", TypeName = "int")]
        public int? Country_Id { get; set; }
        [Column("longitude", TypeName = "float")]
        public double? Longitude { get; set; }
        [Column("latitude", TypeName = "float")]
        public double? Latitude { get; set; }
        [Column("status", TypeName = "tinyint")]
        public int Status { get; set; }
        [d.Write(false)]
        [Column("timestamp", TypeName = "timestamp")]
        public Byte[] TimeStamp { get; set; }
    }
}

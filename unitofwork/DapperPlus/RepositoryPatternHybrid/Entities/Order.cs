
using d=Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [d.Table("dbo.tb_order")]
    public class Order
    {
        [d.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_Id { get; set; }
        
        public string? Order_Number { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
       
    }
}

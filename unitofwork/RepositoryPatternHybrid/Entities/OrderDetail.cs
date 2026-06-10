using d = Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Entities
{
    [d.Table("dbo.tb_order_details")]
    public class OrderDetail
    {
        [d.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_Detail_Id { get; set; }
        public int Order_Id { get; set; }
        public int Product_Id{ get; set; }
        public int Quantity { get; set; }
    }
}

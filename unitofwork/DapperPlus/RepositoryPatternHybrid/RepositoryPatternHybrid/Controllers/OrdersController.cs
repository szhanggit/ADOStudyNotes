using Microsoft.AspNetCore.Mvc;
using Service.Protos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


public class Order
{
    public int Order_Id { get; set; }

    public string? Order_Number { get; set; }
    public List<OrderDetail>? OrderDetails { get; set; }
}

public class OrderDetail 
{ 
    public int Order_Detail_Id { get; set; }
    public int Order_Id { get; set; }
    public int Product_Id { get; set; }
    public int Quantity { get; set; }
}

namespace RepositoryPatternHybrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        Orders.OrdersClient _cli;
        public OrdersController(Orders.OrdersClient cli)
        {
            _cli = cli;
        }


        // POST api/<OrdersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order request)
        {
            var req = new UpsertOrderRequest
            {
                Entity = new OrderEntity
                {
                    OrderNumber = request.Order_Number
                }
            };

            req.Entity.OrderDetails.AddRange(request.OrderDetails.Select(s => new Service.Protos.OrderDetail
            {
                ProductId = s.Product_Id,
                Quantity = s.Quantity
            }));
            return Ok(await _cli.CreateAsync(req));
        }

        // PUT api/<OrdersController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Order request)
        {
            var req = new UpsertOrderRequest
            {
                Entity = new OrderEntity
                {
                    Id = request.Order_Id,
                    OrderNumber = request.Order_Number
                }
            };
            req.Entity.OrderDetails.AddRange(request.OrderDetails.Select(s => new Service.Protos.OrderDetail
            {
                Id = s.Order_Detail_Id,
                OrderId = s.Order_Id,
                ProductId = s.Product_Id,
                Quantity = s.Quantity
            }));

            _cli.UpdateAsync(req);

            return Ok();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _cli.RemoveAsync(new RemoveOrderRequest { Id = id });
            return Ok(res);
        }
    }
}

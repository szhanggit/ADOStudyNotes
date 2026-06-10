using Entities;
using Grpc.Core;
using Service.Protos;
using System.Transactions;
using static RepositoryDapper.UnitOfWork.SalesUnit;

namespace Service.Services
{
    public class OrderService : Orders.OrdersBase
    {
        ISalesUnitOfWork _unit;
        public OrderService(ISalesUnitOfWork unit, IConfiguration config)
        {
            unit.SetConnection(config["ConnectionStrings:local"]);
            _unit = unit;
        }
        public override async Task<UpsertOrderResponse> Create(UpsertOrderRequest request, ServerCallContext context)
        {

            try
            {
                var order = new Order
                {
                    Order_Number = request.Entity.OrderNumber,
                    Date = DateTime.Now,
                    OrderDetails = request.Entity.OrderDetails.Select(s => new Entities.OrderDetail
                    {
                        Product_Id = s.ProductId,
                        Quantity = s.Quantity,
                    }).ToList()
                };


                _unit.OrderRepo.CustomCreate(order);           

                return new UpsertOrderResponse { Id = 1 };
            }
            catch (Exception ex)
            {
                _unit.Dispose();
                return new UpsertOrderResponse { Id = 0 };
            }

            
        }

        public override async Task<RemoveOrderResponse> Remove(RemoveOrderRequest request, ServerCallContext context)
        {
            var result = new RemoveOrderResponse();
            using (_unit.Context.Transaction = new TransactionScope())
            {
                try
                {
                    _unit.OrderRepo.CustomDelete(request.Id);
                    result.Deleted = true;
                    await _unit.Complete();
                }
                catch (Exception ex)
                {
                    _unit.Dispose();
                    result.Deleted = false;
                }
            }
            return await Task.FromResult(result);
        }

        public override async Task<UpsertOrderResponse> Update(UpsertOrderRequest request, ServerCallContext context)
        {
            var result = new UpsertOrderResponse();
            try
            {
                var order = new Order
                {
                    Order_Id = request.Entity.Id,
                    Order_Number = request.Entity.OrderNumber,
                    Date = DateTime.Now,
                    OrderDetails = request.Entity.OrderDetails.Select(s=> new Entities.OrderDetail
                    {
                        Order_Detail_Id = s.Id,
                        Order_Id = s.OrderId,
                        Product_Id = s.ProductId,
                        Quantity = s.Quantity
                    }).ToList()
                };

                _unit.OrderRepo.CustomUpdate(order);
                   
                result.Id = order.Order_Id;
                    
            }
            catch (Exception ex)
            {
                _unit.Dispose();
                result.Id = 0;
            }
            return result;
        }
    }
}

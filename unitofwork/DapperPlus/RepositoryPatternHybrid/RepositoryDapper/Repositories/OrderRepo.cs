//using DapperExtensions;
using Dapper.Contrib.Extensions;
using Entities;
using TXC.Common.RepositoryCore;
using Z.Dapper.Plus;

namespace RepositoryDapper.Repositories
{
    public class OrderRepo
    {
        public interface IOrderRepository : IRepository<Order>
        {
            void CustomUpdate(Order order);
            void CustomCreate(Order order);
            void CustomDelete(int id);
        }

        public class OrderRepository : Repository<Order>, IOrderRepository
        {
            
            public OrderRepository(Context context) : base(context)
            {
            }

            public void CustomCreate(Order order)
            {
    
                    _context.Entity<Order>()
                       .Identity(x => x.Order_Id)
                       .Ignore(x => x.OrderDetails)
                       .AfterAction((kind, x) =>
                       {
                           if (kind == DapperPlusActionKind.Insert || kind == DapperPlusActionKind.Merge)
                               x.OrderDetails.ForEach(f => f.Order_Id = x.Order_Id);
                       });
                    _context
                        
                        .BulkInsert(order, order => order.OrderDetails);
               
               
            }

            public void CustomUpdate(Order order)
            {
               
                    _context.Entity<Order>()
                    .Identity(x => x.Order_Id)
                    .Ignore(x => x.OrderDetails);
                    _context
                    .UseBulkOptions(o => o.InsertIfNotExists = true)
                    .BulkMerge(order, o => o.OrderDetails);
              
                

            }

            public void CustomDelete(int id)
            {
                //var orderDetails = _context.Connection.GetList<OrderDetail>(Predicates.Field<OrderDetail>(f => f.Order_Id, DapperExtensions.Predicate.Operator.Eq, id));
                var orderDetails = _context.Connection.GetAll<OrderDetail>().Where(w=> w.Order_Id == id);                
                _context.BulkDelete(orderDetails);

                Remove(_context.Connection.Get<Order>(id)).Wait();

            }

        }
    }
}

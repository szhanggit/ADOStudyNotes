using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.RepositoryCore;
using Z.Dapper.Plus;

namespace RepositoryDapper.Repositories
{
    public class OrderDetailRepo
    {
        public interface IOrderDetailRepository : IRepository<OrderDetail>
        {
            Task Upsert(List<OrderDetail> entities);
        }
        public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
        {
            public OrderDetailRepository(Context context) : base(context)
            {
            }


            public async Task Upsert(List<OrderDetail> entities)
            {
                await Task.Factory.StartNew(() =>
                {
                    const string ignoreProp = "OrderDetail_IgnoreProp";
                    //_context.Entity<OrderDetail>(ignoreProp).IgnoreOnMergeUpdate(i => new { i.Order_Id, i.Product_Id });
                    _context.Entity<OrderDetail>(ignoreProp).IgnoreOnMergeUpdate(i => new { i.Order_Detail_Id, i.Order_Id, i.Product_Id });
                    _context.BulkMerge(ignoreProp, entities);
                });                
            }
        }
    }
}

using Dapper.Contrib.Extensions;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.RepositoryCore;

namespace RepositoryDapper.Repositories
{
    public class ProductRepo
    {
        public interface IProductRepository : IRepository<Product>
        {
            Task<bool> DeleteProduct(int id);
        }

        public class ProductRepository : Repository<Product>, IProductRepository
        {
            public ProductRepository(Context context) : base(context)
            {
            }

            public async Task<bool> DeleteProduct(int id)
            {
                return await Remove(_context.Connection.Get<Product>(id));
            }
        }

    }
}

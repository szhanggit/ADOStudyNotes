using TXC.Common.RepositoryCore;
using static RepositoryDapper.Repositories.OrderDetailRepo;
using static RepositoryDapper.Repositories.OrderRepo;
using static RepositoryDapper.Repositories.ProductRepo;

namespace RepositoryDapper.UnitOfWork
{
    public class SalesUnit
    {
        public interface ISalesUnitOfWork : IUnitOfWork
        {
            IProductRepository ProductRepo { get; }
            IOrderRepository OrderRepo { get; }
            IOrderDetailRepository OrderDetailRepo { get; }
            Context Context { get; }
        }

        public class SalesUnitOfWork : ISalesUnitOfWork
        {
            public IProductRepository ProductRepo { get; private set; }

            public IOrderRepository OrderRepo { get; private set; }

            public IOrderDetailRepository OrderDetailRepo { get; private set; }

            private readonly Context _context;
            public SalesUnitOfWork(Context context)
            {
                _context = context;
                ProductRepo = new ProductRepository(context);
                OrderDetailRepo = new OrderDetailRepository(context);
                OrderRepo = new OrderRepository(context);
            }

            public Context Context => _context;

            public async Task<int> Complete()
            {
                if (_context.Transaction == null)
                    throw new ArgumentNullException(nameof(_context.Transaction));

                _context.Transaction.Complete();
                return await Task.FromResult(0);
            }

            public void Dispose()
            {
                _context.Dispose();
            }

            public void SetConnection(string connectionString)
            {
                _context.SetConnection(connectionString);
            }
        }
    }
}

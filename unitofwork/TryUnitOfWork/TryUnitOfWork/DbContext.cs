namespace TryUnitOfWork
{
    public class DbContext : IDbContext
    {
        private IUnitOfWorkFactory unitOfWorkFactory;

        private UnitOfWork unitOfWork;

        private IProductRepository product;

        public DbContext(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public IProductRepository Product =>
            product ?? (product = new ProductRepository(UnitOfWork));

        protected UnitOfWork UnitOfWork =>
            unitOfWork ?? (unitOfWork = unitOfWorkFactory.Create());

        public void Commit()
        {
            try
            {
                UnitOfWork.Commit();
            }
            finally
            {
                Reset();
            }
        }

        public void Rollback()
        {
            try
            {
                UnitOfWork.Rollback();
            }
            finally
            {
                Reset();
            }
        }

        private void Reset()
        {
            unitOfWork = null;
            product = null;
        }
    }
}

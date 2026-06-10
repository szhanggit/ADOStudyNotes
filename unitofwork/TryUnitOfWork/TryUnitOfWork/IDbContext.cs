namespace TryUnitOfWork
{
    public interface IDbContext
    {
        IProductRepository Product { get; }

        void Commit();
        void Rollback();
    }
}

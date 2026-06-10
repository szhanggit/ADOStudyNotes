namespace TryUnitOfWork
{
    public interface IProductRepository
    {
        Product Read(int id);
        Task<Product> ReadAsync(int id);
    }
}

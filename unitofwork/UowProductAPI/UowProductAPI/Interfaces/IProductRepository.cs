using UowProductAPI.Models;

namespace UowProductAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<int> AddAsync(Product entity);
        Task<int> DeleteAsync(int id);
        Task<IReadOnlyList<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<int> UpdateAsync(Product entity);

    }
}

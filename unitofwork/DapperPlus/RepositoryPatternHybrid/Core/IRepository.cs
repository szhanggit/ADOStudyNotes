namespace Core
{
    public interface IRepository<T> where T : class, new()
    {
        Task<int> Add(T entity);
        Task<bool> Remove(T entity);
        Task<bool> Update(T entity);
    }
}
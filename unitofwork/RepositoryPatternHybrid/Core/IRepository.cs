namespace Core
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IEnumerable<T>> Get();
        Task Add(T entity);
        Task Remove(T entity);
        Task Update(T entity);
    }
}
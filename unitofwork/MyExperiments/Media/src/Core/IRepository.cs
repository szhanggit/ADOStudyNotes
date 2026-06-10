namespace Core
{
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// Get all data set of entity
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> Get();
        /// <summary>
        /// Add new record to database
        /// </summary>
        /// <param name="entity">Your entity object</param>
        /// <returns></returns>
        Task Add(T entity);
        /// <summary>
        /// Remove record to database by entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Remove(T entity);
        /// <summary>
        /// Use to modify or update your record in database
        /// </summary>
        /// <param name="entity">The updated value of entity object</param>
        /// <returns></returns>
        Task Update(T entity);
    }
}

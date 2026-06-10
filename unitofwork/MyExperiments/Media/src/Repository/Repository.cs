
using Dapper.Contrib.Extensions;
using TXC.Common.RepositoryCore;

namespace Repository
{
    /// <summary>
    /// Generic Repository Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T>
            where T : class, new()
    {
        protected Context _context;
        public Repository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Use to add entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await _context.Connection.InsertAsync(entity);
        }


        /// <summary>
        /// Use to remove record by entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await _context.Connection.DeleteAsync(entity);
        }

        /// <summary>
        /// Use to update record by entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await _context.Connection.UpdateAsync(entity);
        }
    }
}
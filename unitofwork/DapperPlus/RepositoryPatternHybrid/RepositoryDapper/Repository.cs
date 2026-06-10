
using Dapper.Contrib.Extensions;
using TXC.Common.RepositoryCore;
using s = System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryDapper
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected Context _context;
        public Repository(Context context)
        {
            _context = context;
        }


        public async Task<int> Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await _context.Connection.InsertAsync(entity);            
        }
        public async Task<bool> Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await _context.Connection.DeleteAsync(entity);
        }

        public async Task<bool> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await _context.Connection.UpdateAsync(entity);
        }

    }
}
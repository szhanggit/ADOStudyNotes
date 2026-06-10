using Core;
using Microsoft.EntityFrameworkCore;

namespace RepositoryEf
{
    public class Repository<T> : IRepository<T>
        where T : class, new()
    {
        protected readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _context.AddAsync(entity);
        }
        public async Task Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await Task.FromResult(_context.Remove(entity));
        }
        public async Task Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await Task.FromResult(_context.Update(entity));
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await Task.FromResult(_context.Set<T>().AsNoTracking());
        }

    }
}
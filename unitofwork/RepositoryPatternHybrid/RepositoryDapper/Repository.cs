using Core;
using Dapper.Contrib.Extensions;
using s = System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryDapper
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected MediaContext _context;
        public Repository(MediaContext context)
        {
            _context = context;
        }

        s.TableAttribute TbAttribute => (s.TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(s.TableAttribute));

        public async Task Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _context.Entity<T>().Table($"{TbAttribute.Schema}.{TbAttribute.Name}");
            await Task.FromResult(_context.BulkInsert(entity));
            //await _context.Connection.InsertAsync(entity);            
        }
        public async Task Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _context.Connection.DeleteAsync(entity);
        }

        public async Task Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _context.Connection.UpdateAsync(entity);
        }
        public async Task<IEnumerable<T>> Get()
        {
            return await _context.Connection.GetAllAsync<T>();
        }

    }
}
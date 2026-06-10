using Core;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Repository<T> : IRepository2<T> where T : class, new()
    {
        protected Context _context;
        public Repository(Context context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(T entity, IDbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            int id = await _context.Connection.InsertAsync(entity, trans);
            return id;
        }
        public async Task<bool> RemoveAsync(T entity, IDbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            bool flag = await _context.Connection.DeleteAsync(entity, trans);
            return flag;
        }
        public async Task<bool> UpdateAsync(T entity, IDbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            bool flag = await _context.Connection.UpdateAsync(entity, trans);
            return flag;
        }
        public async Task<T> GetAsync(int Id)
        {
            T item = await _context.Connection.GetAsync<T>(Id);
            return item;
        }
    }
}

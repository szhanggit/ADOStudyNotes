using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IRepository2<T> where T : class, new()
    {
        Task<T> GetAsync(int Id);
        Task<int> AddAsync(T entity, IDbTransaction trans);
        Task<bool> RemoveAsync(T entity, IDbTransaction trans);
        Task<bool> UpdateAsync(T entity, IDbTransaction trans);
    }
}

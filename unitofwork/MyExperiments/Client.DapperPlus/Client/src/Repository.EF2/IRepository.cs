using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF2
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IEnumerable<T>> Get();
        Task Add(T entity);
        Task Remove(T entity);
        Task Update(T entity);
    }
}

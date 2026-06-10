using Core;
using Dapper.Contrib.Extensions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IDictionaryRepository : IRepository2<Dictionary>
    {
        IEnumerable<Dictionary> GetDictionaryLazily();
    }
    public class DictionaryRepository : Repository<Dictionary>, IDictionaryRepository
    {
        public DictionaryRepository(Context context) : base(context)
        {

        }

        public IEnumerable<Dictionary> GetDictionaryLazily()
        {
            IEnumerable<Dictionary> _dictionaryList = _context.Connection.GetAll<Dictionary>().Where(_ => _.Dictionary_Id > 0);
            return _dictionaryList;
        }
    }
}

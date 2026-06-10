using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF
{
    public interface IDictionaryRepository
    {
        Task<List<ProvinceCityPair>> GetProvinceCityPairAsync(int CountryId);
    }
    internal class DictionaryRepository : Repository<ProvinceCityPair>, IDictionaryRepository
    {
        public DictionaryRepository(ClientContext context) : base(context)
        {

        }

        public async Task<List<ProvinceCityPair>> GetProvinceCityPairAsync(int CountryId)
        {
            List<ProvinceCityPair> _list = new List<ProvinceCityPair>();
            string sql = $@"
	                        select 
	                        province.dictionary_id as province, 
	                        city.dictionary_id as city
	                        from [general].[tb_d_dictionary] country with(nolock)
	                        inner join [general].[tb_d_dictionary] province with(nolock) on province.parent_id = country.dictionary_id 
																	                        AND province.category = 'StateOrProvince'
	                        inner join [general].[tb_d_dictionary] city with(nolock) on city.parent_id = province.dictionary_id 
																	                        AND city.category = 'City'
	                        where country.category = 'Country' AND country.dictionary_id = @CountryId
            ";

            var param = new SqlParameter[]
            {
                new SqlParameter("@CountryId", CountryId)
            };
            IQueryable<ProvinceCityPair> result = await Task.FromResult(_context
                .Set<ProvinceCityPair>()
                .FromSqlRaw(sql, param));
            _list = result.AsEnumerable<ProvinceCityPair>().ToList();
            return _list;
        }
    }
}

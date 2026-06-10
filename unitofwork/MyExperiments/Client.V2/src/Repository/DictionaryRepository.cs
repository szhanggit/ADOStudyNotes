using Dapper;
using Domain.Entities;
using Domain.Models;
using System.Data;

namespace Repository
{
    public interface IDictionaryRepository
    {
        Task<List<ProvinceCityPair>> GetProvinceCityPairAsync(int CountryId);
    }
    internal class DictionaryRepository : IDictionaryRepository
    {
        private IUnitOfWork unitOfWork = null;
        public DictionaryRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<ProvinceCityPair>> GetProvinceCityPairAsync(int CountryId)
        {
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

            List<ProvinceCityPair> _list = new List<ProvinceCityPair>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CountryId", CountryId, DbType.Int32, ParameterDirection.Input);
            var result = await unitOfWork.Connection.QueryAsync<ProvinceCityPair>(sql, parameters, unitOfWork.Transaction);
            if (result != null)
            {
                _list = result.ToList();
            }

            return _list;
        }
    }
}

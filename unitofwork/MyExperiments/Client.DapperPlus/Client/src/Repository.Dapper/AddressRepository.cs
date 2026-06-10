using Core;
using Dapper;
using Domain.Entities;
using System.Data;

namespace Repository.Dapper
{
	public interface IAddressRepository
	{
		Task<Address> GetAddressById(int AddressId);
		Task<int?> CreateAddressAsync(Address address);
		Task UpdateAddressAsync(Address address);
	}

	internal class AddressRepository : IAddressRepository
	{
		private IUnitOfWork unitOfWork = null;
		public AddressRepository(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task<Address> GetAddressById(int AddressId)
		{
			string sql = $@"
			                select 
				            detail_address_line as DetailAddressLine, 
				            district as District, 
				            city_id as CityId, 
				            state_province_id as StateOrProvinceId, 
				            postcode as Postcode, 
				            country_id as CountryId, 
				            longitude as Longitude, 
				            latitude as Latitude, 
				            [Status] as AddressStatus
							[general].[tb_a_address] with(nolock)
							where address_id = @AddressId
            ";

			Address address = new Address();
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AddressId", AddressId, DbType.Int32, ParameterDirection.Input);

			var result = await unitOfWork.Connection.QueryAsync<Address>(sql, parameters, unitOfWork.Transaction);
			if (result != null)
			{
				address = result.FirstOrDefault();
			}

			return address;
		}

		public async Task<int?> CreateAddressAsync(Address address)
		{
			string sql = $@"
			                insert into [general].[tb_a_address]( 
				                [detail_address_line]
				                , [district]
				                , [city_id]
				                , [state_province_id]
				                , [postcode]
				                , [country_id]
				                , [longitude]
				                , [latitude]
				                , [Status]) values (
				                @DetailAddressLine
				                , @District
				                , @CityId
				                , @StateOrProvinceId
				                , @Postcode
				                , @CountryId
				                , @Longitude
				                , @Latitude
				                , @AddressStatus
				                );
			                set @AddressId = SCOPE_IDENTITY();
            ";

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@DetailAddressLine", address.DetailAddressLine, DbType.String, ParameterDirection.Input);
			parameters.Add("@District", address.District, DbType.String, ParameterDirection.Input);
			parameters.Add("@CityId", address.CityId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("@StateOrProvinceId", address.StateOrProvinceId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("@Postcode", address.Postcode, DbType.String, ParameterDirection.Input);
			parameters.Add("@CountryId", address.CountryId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("@Longitude", address.Longitude, DbType.Double, ParameterDirection.Input);
			parameters.Add("@Latitude", address.Latitude, DbType.Double, ParameterDirection.Input);
			parameters.Add("@AddressStatus", address.AddressStatus, DbType.Int32, ParameterDirection.Input);
			parameters.Add("@AddressId", 0, DbType.Int32, ParameterDirection.Output);

			await unitOfWork.Connection.ExecuteScalarAsync<int>(sql, parameters, unitOfWork.Transaction);
			int? AddressId = parameters.Get<int?>("@AddressId");
			return AddressId;
		}

		public async Task UpdateAddressAsync(Address address)
		{
			string sql = $@"
			    update [general].[tb_a_address]
				    set [detail_address_line] = @DetailAddressLine
				    , [district] = @District
				    , [city_id] = @CityId
				    , [state_province_id] = @StateOrProvinceId
				    , [postcode] = @Postcode
				    , [country_id] = @CountryId
				    , [longitude] = @Longitude
				    , [Latitude] = @Latitude
				    , [Status] = @AddressStatus
			    where address_id = @AddressId
            ";

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@DetailAddressLine", address.DetailAddressLine, DbType.String, ParameterDirection.Input);
			parameters.Add("@District", address.District, DbType.String, ParameterDirection.Input);
			parameters.Add("@CityId", address.CityId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("@StateOrProvinceId", address.StateOrProvinceId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("@Postcode", address.Postcode, DbType.String, ParameterDirection.Input);
			parameters.Add("@CountryId", address.CountryId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("@Longitude", address.Longitude, DbType.Double, ParameterDirection.Input);
			parameters.Add("@Latitude", address.Latitude, DbType.Double, ParameterDirection.Input);
			parameters.Add("@AddressStatus", address.AddressStatus, DbType.Int32, ParameterDirection.Input);

			await unitOfWork.Connection.ExecuteScalarAsync(sql, parameters, unitOfWork.Transaction);
		}
	}
}
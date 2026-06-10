

DROP PROCEDURE IF EXISTS [client].[sp_sel_address_by_country]
GO


CREATE PROCEDURE [client].[sp_sel_address_by_country]
	@CountryId int 
	, @ProvinceId int 
	, @CityId int 
	, @ErrorCode int output -- 1 = invalid country / 2 = invalid province / 3 = invalid city / 0 = all valid

AS 

BEGIN
	set @ErrorCode = 0   

	drop table if exists #tempProvinceCityPair

	select 
	province.dictionary_id as province, 
	city.dictionary_id as city
	into #tempProvinceCityPair
	from [general].[tb_d_dictionary] country with(nolock)
	inner join [general].[tb_d_dictionary] province with(nolock) on province.parent_id = country.dictionary_id 
																	AND province.category = 'StateOrProvince'
	inner join [general].[tb_d_dictionary] city with(nolock) on city.parent_id = province.dictionary_id 
																	AND city.category = 'City'
	where country.category = 'Country' AND country.dictionary_id = @CountryId

	if(@@rowcount = 0)
	BEGIN
		set @ErrorCode = 1;
		return;
	END
	
	select * from #tempProvinceCityPair
	where province = @ProvinceId

	if(@@rowcount = 0)
	BEGIN
		set @ErrorCode = 2;
		return;
	END

	select * from #tempProvinceCityPair
	where province = @ProvinceId and city = @CityId

	if(@@rowcount = 0)
	BEGIN
		set @ErrorCode = 3;
		return;
	END
END
GO
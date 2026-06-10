using Dapper;
using System.Data;
using TXC.Common.Data;
using TXC.Common.MessageContract;
using TXC.Proto.Client;
using Domain.Models;
using Infrastructure.Extensions;

namespace Repository
{
    public interface IClientRepository
    {
        Task<string> GenerateClientIdentityAsync(GenerateClientIdentityCodeModel request, IDbConnection _dbConnection);
        Task<int?> InsertClientAsync(CreateClientRequest request, string identityCode, IDbConnection _dbConnection);
        Task<int> UpdateClientAsync(UpdateClientRequest request, IDbConnection _dbConnection);
        Task<Tuple<int, IEnumerable<ClientListItem>>> GetClientAsync(GetClientListRequest request, IDbConnection _dbConnection);
        Task<Tuple<bool, string>> CheckIfValidAddress(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection);
        Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection);
        Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection);
        Task<Tuple<CreateBXPClientResponse, int?>> CreateBXPClientAsync(CreateBXPClientRequest request, string securityKey, string identityCode, IDbConnection _dbConnection);
    }
    public class ClientRepository : IClientRepository
    {
        private readonly IDapperOperation _dapperOperation;
        private readonly ITX2ServiceBusSender _txcServiceBusSender;
        public ClientRepository()
        {

        }

        public ClientRepository(
            IDapperOperation dapperOperation
            , ITX2ServiceBusSender txcServiceBusSender)
        {
            _dapperOperation = dapperOperation;
            _txcServiceBusSender = txcServiceBusSender;
        }
         
        public async Task<string> GenerateClientIdentityAsync(GenerateClientIdentityCodeModel request, IDbConnection _dbConnection)
        {
            string sql = $@"
              declare @sql as nvarchar(500)
              declare @nextvalue as bigint
              select @sql = N'select @nextvalue = next value for ' + @SequenceName;
              exec SP_EXECUTESQL @sql, N'@nextvalue bigint out', @nextvalue output
              if(@IsFixReturnLength = 1)
              begin
                  set @ret = right(replicate(@PaddingCharacter,@ReturnLength)+CAST(@nextvalue as varchar(100)),@ReturnLength)
              end
              else
              begin
                  set @ret = CAST(@nextvalue as varchar(100))
              end
              select @ret;
            ";

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@SequenceName", request.SequenceName, DbType.AnsiString, ParameterDirection.Input);
            parameters.Add("@IsFixReturnLength", request.IsFixReturnLength, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@ReturnLength", request.ReturnLength, DbType.Int16, ParameterDirection.Input);
            parameters.Add("@PaddingCharacter", request.PaddingCharacter, DbType.AnsiString, ParameterDirection.Input);
            parameters.Add("@ret", string.Empty, DbType.AnsiString, ParameterDirection.Output);

            CommandDefinition commandDefinition = new CommandDefinition(sql, commandType: CommandType.Text,
                                                                    parameters: parameters, cancellationToken: default);

            var dbResult = await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<string>, string>(_dbConnection, commandDefinition);
            return dbResult;
        }

        public async Task<int?> InsertClientAsync(CreateClientRequest request, string identityCode, IDbConnection _dbConnection)
        {
            string sql = $@"
	                        SET XACT_ABORT ON;

                            BEGIN TRY

                                BEGIN TRANSACTION;
        
		                        Declare @AddressId int = NULL;

		                        if (@CountryId is not NULL)
		                        begin
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
		                        end

		                        INSERT INTO [client].[tb_cbi_client_basic_information]
                                    ([client_name]
                                    ,[identity_code]
                                    ,[voucher_issuer_id]
                                    ,[invoice_register_number]
                                    ,[business_type_id]
                                    ,[status]
                                    ,[security_algorithm]
                                    ,[security_key]
                                    ,[need_notification]
                                    ,[notification_provider_code_id]
                                    ,[logo_media_id]
                                    ,[banner_media_id]
                                    ,[email_header_media_id]
                                    ,[email_footer_media_id]
                                    ,[can_issue]
                                    ,[mandatory_auto_billing]
                                    ,[invoice_title]
                                    ,[sub_url]
                                    ,[email_provider_code]
                                    ,[email_sender_name]
                                    ,[email_sender_address]
                                    ,[apply_email_subject]
                                    ,[sms_provider_code]
                                    ,[sms_sender_name]
                                    ,[sms_entity_id]
                                    ,[sales_email]
                                    ,[contact_name]
                                    ,[contact_email]
                                    ,[contact_phone]
                                    ,[Memo]
                                    ,[description]
			                        ,[address_id])
                             VALUES
                                    (@ClientName 
                                    ,@IdentityCode 
	                                ,@VoucherIssuerId 
	                                ,@InvoiceRegisterNumber 
                                    ,@BusinessTypeId 
                                    ,@Status 
                                    ,@SecurityAlgorithm 
                                    ,@SecurityKey 
                                    ,@NeedNotification 
                                    ,@NotificationProviderCodeId 
                                    ,@LogoMediaId 
	                                ,@BannerMediaId 
	                                ,@EmailHeaderMediaId 
                                    ,@EmailFooterMediaId 
	                                ,@CanIssue 
                                    ,@MandatoryAutoBilling 
                                    ,@InvoiceTitle 
                                    ,@SubURL 
	                                ,@EmailProviderCode 
                                    ,@EmailSenderName 
                                    ,@EmailSenderAddress 
	                                ,@ApplyEmailSubject 
	                                ,@SmsProviderCode 
                                    ,@SMSSenderName 
                                    ,@SmsEntityId 
                                    ,@SalesEmail 
                                    ,@ContactName 
                                    ,@ContactEmail 
                                    ,@ContactPhone 
                                    ,@Memo
                                    ,@Description
			                        ,@AddressId)

                                COMMIT TRANSACTION;

	                        set @ClientId = SCOPE_IDENTITY();
                            END TRY
                            BEGIN CATCH 

                               IF @@TRANCOUNT  > 0
                               BEGIN
                                  ROLLBACK TRANSACTION
                               END;

	                           THROW;
                            END CATCH;

                            SET NOCOUNT, XACT_ABORT OFF;
            ";

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ClientName", request.ClientName, DbType.String, ParameterDirection.Input);
            parameters.Add("@IdentityCode", identityCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceRegisterNumber", request.InvoiceRegisterNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@VoucherIssuerId", request.VoucherIssuerId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BusinessTypeId", request.BusinessTypeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Status", request.Status, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SecurityAlgorithm", request.SecurityAlgorithm, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SecurityKey", request.SecurityKey, DbType.String, ParameterDirection.Input);
            parameters.Add("@NeedNotification", request.NeedNotification, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@NotificationProviderCodeId", request.NotificationProviderCodeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@LogoMediaId", request.LogoMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BannerMediaId", request.BannerMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailHeaderMediaId", request.EmailHeaderMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailFooterMediaId", request.EmailFooterMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CanIssue", request.CanIssue, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@MandatoryAutoBilling", request.MandatoryAutoBilling, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@InvoiceTitle", request.InvoiceTitle, DbType.String, ParameterDirection.Input);
            parameters.Add("@SubURL", request.SubUrl, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailProviderCode", request.EmailProviderCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailSenderName", request.EmailSenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailSenderAddress", request.EmailSenderAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@ApplyEmailSubject", request.ApplyEmailSubject, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@SMSProviderCode", request.SmsProviderCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@SMSSenderName", request.SmsSenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@SmsEntityId", request.SmsEntityId, DbType.String, ParameterDirection.Input);
            parameters.Add("@SalesEmail", request.SalesEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactName", request.ContactName, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactEmail", request.ContactEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactPhone", request.ContactPhone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Memo", request.Memo, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", request.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@DetailAddressLine", request.DetailAddressLine, DbType.String, ParameterDirection.Input);
            parameters.Add("@District", request.District, DbType.String, ParameterDirection.Input);
            parameters.Add("@CityId", request.CityId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@StateOrProvinceId", request.StateOrProvinceId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Postcode", request.Postcode, DbType.String, ParameterDirection.Input);
            parameters.Add("@CountryId", request.CountryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Longitude", request.Longitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Latitude", request.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@AddressStatus", request.AddressStatus, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ClientId", 0, DbType.Int32, ParameterDirection.Output);

            CommandDefinition commandDefinition = new CommandDefinition(sql, commandType: CommandType.Text,
                                                                    parameters: parameters, cancellationToken: default);

            await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<int>, int>(_dbConnection, commandDefinition);
            int? ClientId = parameters.Get<int?>("@ClientId");
            return ClientId;
        }

        public async Task<int> UpdateClientAsync(UpdateClientRequest request, IDbConnection _dbConnection)
        {
            string sql = $@"
	                        SET XACT_ABORT ON;

                            BEGIN TRY

                                BEGIN TRANSACTION;       
		                        Declare @AddressId int = NULL;

		                        UPDATE [client].[tb_cbi_client_basic_information]
		                           SET [client_name] = @ClientName
			                          ,[voucher_issuer_id] = @VoucherIssuerId
			                          ,[invoice_register_number] = @InvoiceRegisterNumber
			                          ,[business_type_id] = @BusinessTypeId
			                          ,[status] = @Status
			                          ,[security_algorithm] = @SecurityAlgorithm
			                          ,[security_key] = @SecurityKey
			                          ,[need_notification] = @NeedNotification
			                          ,[notification_provider_code_id] = @NotificationProviderCodeId
			                          ,[logo_media_id] = @LogoMediaId
			                          ,[banner_media_id] = @BannerMediaId
			                          ,[email_header_media_id] = @EmailHeaderMediaId
			                          ,[email_footer_media_id] = @EmailFooterMediaId
			                          ,[can_issue] = @CanIssue
			                          ,[mandatory_auto_billing] = @MandatoryAutoBilling
			                          ,[invoice_title] = @InvoiceTitle
			                          ,[sub_url] = @SubURL
			                          ,[email_provider_code] = @EmailProviderCode
			                          ,[email_sender_name] = @EmailSenderName
			                          ,[email_sender_address] = @EmailSenderAddress 
			                          ,[apply_email_subject] = @ApplyEmailSubject
			                          ,[sms_provider_code] = @SmsProviderCode
			                          ,[sms_sender_name] = @SMSSenderName
			                          ,[sms_entity_id] = @SmsEntityId
			                          ,[sales_email] = @SalesEmail
			                          ,[contact_name] = @ContactName
			                          ,[contact_email] = @ContactEmail
			                          ,[contact_phone] = @ContactPhone
			                          ,[Memo] = @Memo
                                      ,[description] = @Description
		                         WHERE client_id = @ClientId;

		                         select @AddressId = address_id from [client].[tb_cbi_client_basic_information] with(nolock) where client_id = @ClientId;

		                        if (@CountryId is not NULL)
		                        begin
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
		                        end

                                COMMIT TRANSACTION;
	
		                        SELECT @@ROWCOUNT;
                            END TRY
                            BEGIN CATCH 

                               IF @@TRANCOUNT  > 0
                               BEGIN
                                  ROLLBACK TRANSACTION
                               END;

	                           THROW;
                            END CATCH;

                            SET NOCOUNT, XACT_ABORT OFF;
            ";

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ClientId", request.ClientId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ClientName", request.ClientName, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceRegisterNumber", request.InvoiceRegisterNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@VoucherIssuerId", request.VoucherIssuerId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BusinessTypeId", request.BusinessTypeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Status", request.Status, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SecurityAlgorithm", request.SecurityAlgorithm, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SecurityKey", request.SecurityKey, DbType.String, ParameterDirection.Input);
            parameters.Add("@NeedNotification", request.NeedNotification, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@NotificationProviderCodeId", request.NotificationProviderCodeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@LogoMediaId", request.LogoMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BannerMediaId", request.BannerMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailHeaderMediaId", request.EmailHeaderMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailFooterMediaId", request.EmailFooterMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CanIssue", request.CanIssue, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@MandatoryAutoBilling", request.MandatoryAutoBilling, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@InvoiceTitle", request.InvoiceTitle, DbType.String, ParameterDirection.Input);
            parameters.Add("@SubURL", request.SubUrl, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailProviderCode", request.EmailProviderCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailSenderName", request.EmailSenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailSenderAddress", request.EmailSenderAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@ApplyEmailSubject", request.ApplyEmailSubject, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@SMSProviderCode", request.SmsProviderCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@SMSSenderName", request.SmsSenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@SmsEntityId", request.SmsEntityId, DbType.String, ParameterDirection.Input);
            parameters.Add("@SalesEmail", request.SalesEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactName", request.ContactName, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactEmail", request.ContactEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactPhone", request.ContactPhone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Memo", request.Memo, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", request.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@DetailAddressLine", request.DetailAddressLine, DbType.String, ParameterDirection.Input);
            parameters.Add("@District", request.District, DbType.String, ParameterDirection.Input);
            parameters.Add("@CityId", request.CityId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@StateOrProvinceId", request.StateOrProvinceId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Postcode", request.Postcode, DbType.String, ParameterDirection.Input);
            parameters.Add("@CountryId", request.CountryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Longitude", request.Longitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Latitude", request.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@AddressStatus", request.AddressStatus, DbType.Int32, ParameterDirection.Input);

            CommandDefinition commandDefinition = new CommandDefinition(sql, commandType: CommandType.Text,
                                                                    parameters: parameters, cancellationToken: default);

            int dbaffectedRows = await _dapperOperation.ProcessSql<ExecuteCommand, int>(_dbConnection, commandDefinition);
            return dbaffectedRows;
        }

        public async Task<Tuple<int, IEnumerable<ClientListItem>>> GetClientAsync(GetClientListRequest request, IDbConnection _dbConnection)
        {
            string sql = $@"
	                        IF(@ClientId > 0)
                            BEGIN
                                        SELECT c.[client_id] AS ClientId,
				                        c.[client_name] AS ClientName,
				                        c.[identity_code] AS IdentityCode,
				                        c.[voucher_issuer_id] AS VoucherIssuerId,
				                        c.[invoice_register_number] AS InvoiceRegisterNumber,
				                        c.[business_type_id] AS BusinessTypeId,
				                        c.[status] ,
				                        c.[security_algorithm] AS SecurityAlgorithm,
				                        c.[security_key] AS SecurityKey,
				                        c.[need_notification] AS NeedNotification,
				                        c.[notification_provider_code_id] AS NotificationProviderCodeId,
				                        c.[logo_media_id] AS LogoMediaId,
				                        c.[banner_media_id] AS BannerMediaId,
				                        c.[email_header_media_id] AS EmailHeaderMediaId,
				                        c.[email_footer_media_id] AS EmailFooterMediaId,
				                        c.[can_issue] AS CanIssue,
				                        c.[mandatory_auto_billing] AS MandatoryAutoBilling,
				                        c.[invoice_title] AS InvoiceTitle,
				                        c.[sub_url] AS SubURL,
				                        c.[email_provider_code] AS EmailProviderCode,
				                        c.[email_sender_name] AS EmailSenderName,
				                        c.[email_sender_address] AS EmailSenderAddress,
				                        c.[apply_email_subject] AS ApplyEmailSubject,
				                        c.[sms_provider_code] AS SMSProviderCode,
				                        c.[sms_sender_name] AS SMSSenderName,
				                        c.[sms_entity_id] AS SMSEntityId,
				                        c.[sales_email] AS SalesEmail,
				                        c.[contact_name] AS ContactName,
				                        c.[contact_email] AS ContactEmail,
				                        c.[contact_phone] AS ContactPhone,
				                        c.[Memo],
                                        c.[description] AS [Description],
				                        addre.detail_address_line as DetailAddressLine, 
				                        addre.district as District, 
				                        addre.city_id as CityId, 
				                        addre.state_province_id as StateOrProvinceId, 
				                        addre.postcode as Postcode, 
				                        addre.country_id as CountryId, 
				                        addre.longitude as Longitude, 
				                        addre.latitude as Latitude, 
				                        addre.[Status] as AddressStatus
			                        FROM [client].[tb_cbi_client_basic_information] c with(nolock)
			                        inner join [general].[tb_a_address] addre with(nolock) on c.address_id = addre.address_id
			                        where c.client_id = @ClientId;

                                SET @TotalCount = 1;
                            END
                            ELSE
                            BEGIN
                                create table #Tmp 
                                (
                                    ID int NOT NULL
                                );
        
                                INSERT INTO #Tmp SElECT client_id FROM
                                (
                                    SElECT client_id from client.tb_cbi_client_basic_information c with(nolock) where c.client_name LIKE N'%'+@SearchKeyWord+'%'
                                    Union
                                    SElECT client_id from client.tb_cbi_client_basic_information c with(nolock) where c.identity_code LIKE N'%'+@SearchKeyWord+'%'
                                    Union
                                    SElECT client_id from client.tb_cbi_client_basic_information c with(nolock) where c.invoice_register_number LIKE N'%'+@SearchKeyWord+'%'
                                ) CL

                               SELECT c.[client_id] AS ClientId,
				                        c.[client_name] AS ClientName,
				                        c.[identity_code] AS IdentityCode,
				                        c.[voucher_issuer_id] AS VoucherIssuerId,
				                        c.[invoice_register_number] AS InvoiceRegisterNumber,
				                        c.[business_type_id] AS BusinessTypeId,
				                        c.[status] ,
				                        c.[security_algorithm] AS SecurityAlgorithm,
				                        c.[security_key] AS SecurityKey,
				                        c.[need_notification] AS NeedNotification,
				                        c.[notification_provider_code_id] AS NotificationProviderCodeId,
				                        c.[logo_media_id] AS LogoMediaId,
				                        c.[banner_media_id] AS BannerMediaId,
				                        c.[email_header_media_id] AS EmailHeaderMediaId,
				                        c.[email_footer_media_id] AS EmailFooterMediaId,
				                        c.[can_issue] AS CanIssue,
				                        c.[mandatory_auto_billing] AS MandatoryAutoBilling,
				                        c.[invoice_title] AS InvoiceTitle,
				                        c.[sub_url] AS SubURL,
				                        c.[email_provider_code] AS EmailProviderCode,
				                        c.[email_sender_name] AS EmailSenderName,
				                        c.[email_sender_address] AS EmailSenderAddress,
				                        c.[apply_email_subject] AS ApplyEmailSubject,
				                        c.[sms_provider_code] AS SMSProviderCode,
				                        c.[sms_sender_name] AS SMSSenderName,
				                        c.[sms_entity_id] AS SMSEntityId,
				                        c.[sales_email] AS SalesEmail,
				                        c.[contact_name] AS ContactName,
				                        c.[contact_email] AS ContactEmail,
				                        c.[contact_phone] AS ContactPhone,
				                        c.[Memo],                
                                        c.[description] AS [Description],
				                        addre.detail_address_line as DetailAddressLine, 
				                        addre.district as District, 
				                        addre.city_id as CityId, 
				                        addre.state_province_id as StateOrProvinceId, 
				                        addre.postcode as Postcode, 
				                        addre.country_id as CountryId, 
				                        addre.longitude as Longitude, 
				                        addre.latitude as Latitude, 
				                        addre.[Status] as AddressStatus
                                FROM #Tmp t with(nolock) 
                                INNER JOIN [client].[tb_cbi_client_basic_information] c with(nolock) on t.Id = c.client_id
		                        INNER JOIN [general].[tb_a_address] addre with(nolock) on c.address_id = addre.address_id
                                Order BY c.client_id 
                                OFFSET @PageOffset ROWS
	                            FETCH NEXT @RowCount ROWS ONLY

                                SELECT @TotalCount = count(1) FROM #Tmp;
        
                                DROP TABLE #Tmp
                            END
            ";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RowCount", request.RowCount, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PageOffset", request.GetPageOffset(), DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ClientId", request.ClientId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SearchKeyWord", string.IsNullOrEmpty(request.SearchKeyword) ? "" : request.SearchKeyword, DbType.String, ParameterDirection.Input);
            parameters.Add("@TotalCount", 0, DbType.Int32, ParameterDirection.Output);

            CommandDefinition commandDefinition = new CommandDefinition(sql, commandType: CommandType.Text,
                                                                        parameters: parameters, cancellationToken: default);

            IEnumerable<ClientListItem> dbResult = await _dapperOperation.ProcessSql<SelectMany<ClientListItem>, IEnumerable<ClientListItem>>(_dbConnection, commandDefinition);
            int totalCount = parameters.Get<int>("@TotalCount");
            return Tuple.Create(totalCount, dbResult);
        }

        /// <summary>
        /// Check if address is valid.
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="StateOrProvinceId"></param>
        /// <param name="CountryId"></param>
        /// <param name="_dbConnection"></param>
        /// <returns>Tuple<bool, string>; bool = TRUE if exists. Otherwise, FALSE. string = error message</returns>
        public async Task<Tuple<bool, string>> CheckIfValidAddress(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection)
        {
            string sql = $@"
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
            ";
            CommandDefinition commandDefinition;
            Tuple<bool, string> result = null;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CountryId", CountryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ProvinceId", StateOrProvinceId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CityId", CityId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ErrorCode", 0, DbType.Int32, ParameterDirection.Output);

            commandDefinition = new CommandDefinition(sql, commandType: CommandType.Text,
                                                    parameters: parameters, cancellationToken: default);

            await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<int>, int>(_dbConnection, commandDefinition);
            int? errorCode = parameters.Get<int?>("@ErrorCode");

            if (!errorCode.HasValue)
            {
                result = new Tuple<bool, string>(true, string.Empty);
                return result;
            }

            if (errorCode == 1)
            {
                result = new Tuple<bool, string>(false, "Invalid country id.");
                return result;
            }
            else if (errorCode == 2)
            {
                result = new Tuple<bool, string>(false, "Invalid province id.");
                return result;
            }
            else if (errorCode == 3)
            {
                result = new Tuple<bool, string>(false, "Invalid city id.");
                return result;
            }
            else
            {
                result = new Tuple<bool, string>(true, string.Empty);
                return result;
            }
        }

        public async Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection)
        {
            string sql = $@"select @RowCount = count(*) from [client].[tb_cbi_client_basic_information] with(nolock) where client_id = @ClientId";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ClientId", ClientId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@RowCount", 0, DbType.Int32, ParameterDirection.Output);

            CommandDefinition commandDefinition = new CommandDefinition(sql, commandType: CommandType.Text,
                                                                        parameters: parameters, cancellationToken: default);

            await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<int>, int>(_dbConnection, commandDefinition);
            int RowCount = parameters.Get<int>("@RowCount");
            return RowCount;
        }

        public async Task DeleteClientByIdAsync(int ClientId, IDbConnection _dbConnection)
        {
            string sql = $@"Delete from [client].[tb_cbi_client_basic_information] where client_id = @ClientId";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ClientId", ClientId, DbType.Int32, ParameterDirection.Input);

            CommandDefinition commandDefinition = new CommandDefinition(sql, commandType: CommandType.Text,
                                                                        parameters: parameters, cancellationToken: default);

            await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<int>, int>(_dbConnection, commandDefinition);
        }

        public async Task<Tuple<CreateBXPClientResponse, int?>> CreateBXPClientAsync(CreateBXPClientRequest request, string securityKey, string identityCode, IDbConnection _dbConnection)
        {
            string sql = @"BEGIN  
                    SET XACT_ABORT ON;  

                     IF EXISTS (SELECT 1 from client.tb_cbi_client_basic_information where client_name=trim(@ClientName))
                     Begin
                        set @Errorcode = 1;
                        Return ;
                      End
                      
                     
                    BEGIN TRY  
                    BEGIN TRANSACTION;  
                           Declare @AddressId int = NULL;  
  
                     if (@CountryId is not NULL)  
                      Begin  
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
                        , 1  
                        ); 
            
                         set @AddressId = SCOPE_IDENTITY();  
                        End  
  
                      INSERT INTO [client].[tb_cbi_client_basic_information]  
                                ([client_name]  
                                ,[identity_code]  
                                ,[voucher_issuer_id]  
                                ,[invoice_register_number]  
                                ,[business_type_id]  
                                ,[status]  
                                ,[security_algorithm]  
                                ,[security_key]  
                                ,[need_notification]  
                                ,[notification_provider_code_id]  
                                ,[logo_media_id]  
                                ,[banner_media_id]  
                                ,[email_header_media_id]  
                                ,[email_footer_media_id]  
                                ,[can_issue]  
                                ,[mandatory_auto_billing]  
                                ,[invoice_title]  
                                ,[sub_url]  
                                ,[email_provider_code]  
                                ,[email_sender_name]  
                                ,[email_sender_address]  
                                ,[apply_email_subject]  
                                ,[sms_provider_code]  
                                ,[sms_sender_name]  
                                ,[sms_entity_id]  
                                ,[sales_email]  
                                ,[contact_name]  
                                ,[contact_email]  
                                ,[contact_phone]  
                                ,[Memo]  
                                ,[description]  
                                ,[address_id])  
                         VALUES  
                                (@ClientName   
                                ,@IdentityCode   
                                ,null   
                                ,@InvoiceRegisterNumber   
                                ,null   
                                ,1
                                ,1   
                                ,@SecurityKey   
                                ,0   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,1   
                                ,0   
                                ,@InvoiceTitle   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null   
                                ,null  
                                ,null  
                                ,@AddressId)  
  
                            COMMIT TRANSACTION;  
  
                        set @ClientId = SCOPE_IDENTITY();  
                        END TRY  
                        BEGIN CATCH   
  
                           IF @@TRANCOUNT  > 0  
                           BEGIN  
                              ROLLBACK TRANSACTION  
                           END;  
  
                        THROW;  
                        END CATCH;  
  
                        SET NOCOUNT, XACT_ABORT OFF;  
                    END";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ClientName", request.ClientName, DbType.String, ParameterDirection.Input);
            parameters.Add("@IdentityCode", identityCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceRegisterNumber", request.InvoiceRegisterNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@SecurityKey", securityKey, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceTitle", request.InvoiceTitle, DbType.String, ParameterDirection.Input);
            parameters.Add("@DetailAddressLine", request.DetailAddressLine, DbType.String, ParameterDirection.Input);
            parameters.Add("@District", request.District, DbType.String, ParameterDirection.Input);
            parameters.Add("@CityId", request.CityId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@StateOrProvinceId", request.StateOrProvinceId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Postcode", request.Postcode, DbType.String, ParameterDirection.Input);
            parameters.Add("@CountryId", request.CountryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Longitude", request.Longitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Latitude", request.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@ClientId", 0, DbType.Int32, ParameterDirection.Output);
            parameters.Add("@Errorcode", 0, DbType.Int32, ParameterDirection.Output);

            CommandDefinition commandDefinition = new CommandDefinition(sql, commandType: CommandType.Text,
                                                                    parameters: parameters, cancellationToken: default);

            await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<int>, int>(_dbConnection, commandDefinition);
            int? ClientId = parameters.Get<int?>("@ClientId");
            int? Errorcode = parameters.Get<int?>("@Errorcode");

            if (Errorcode.HasValue && Errorcode == 1)
            {
                return Tuple.Create(new CreateBXPClientResponse() { Success = false, Message = "BXP client already exists." }, ClientId);
            }

            if (!ClientId.HasValue)
            {
                return Tuple.Create(new CreateBXPClientResponse() { Success = false, Message = "Failed to create new BXP client" }, ClientId);
            }

            return Tuple.Create(new CreateBXPClientResponse() { Success = true, Message = "Success" }, ClientId);
        }
    }
}
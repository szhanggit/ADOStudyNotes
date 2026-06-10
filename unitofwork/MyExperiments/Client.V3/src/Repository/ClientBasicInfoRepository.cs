using Dapper;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{

    public interface IClientBasicInfoRepository
    {
        Task<string?> GenerateClientIdentityAsync(GenerateClientIdentityCodeModel request);
        Task<Client> GetClientByIdAsync(int ClientId);
        Task<Tuple<int, List<Client>>> GetClientBySearchKeyAsync(string SearchKeyword, int? RowCount, int? PageNumber);
        Task<int?> CreateClientAsync(Client client);
        Task<int?> CreateBXPClientAsync(Client client);
        Task<int?> UpdateClientAsync(Client client);
        Task DeleteClientById(int ClientId);
    }
    internal class ClientBasicInfoRepository : IClientBasicInfoRepository
    {
        private IUnitOfWork unitOfWork = null;
        public ClientBasicInfoRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<string?> GenerateClientIdentityAsync(GenerateClientIdentityCodeModel request)
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

            IEnumerable<string> ClientCode = await unitOfWork.Connection.QueryAsync<string>(sql, parameters, unitOfWork.Transaction);
            return ClientCode?.FirstOrDefault();
        }

        public async Task<Client> GetClientByIdAsync(int ClientId)
        {
            string sql = $@"
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
                    ";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ClientId", ClientId, DbType.Int32, ParameterDirection.Input);

            Client _client = new Client();
            var result = await unitOfWork.Connection.QueryAsync<Client>(sql, parameters, unitOfWork.Transaction);
            if (result != null)
            {
                _client = result.FirstOrDefault();
            }            
            return _client;
        }

        public async Task<Tuple<int, List<Client>>> GetClientBySearchKeyAsync(string SearchKeyword, int? RowCount, int? PageNumber)
        {
            string sql = $@"
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
            ";

            int PageOffset = (PageNumber >= 1) ? (PageNumber - 1) * RowCount ?? 0 : RowCount ?? 0;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RowCount", RowCount??0, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PageOffset", PageOffset, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SearchKeyWord", string.IsNullOrEmpty(SearchKeyword) ? "" : SearchKeyword, DbType.String, ParameterDirection.Input);
            parameters.Add("@TotalCount", 0, DbType.Int32, ParameterDirection.Output);

            List<Client> clientList = new List<Client>();
            var result = await unitOfWork.Connection.QueryAsync<Client>(sql, parameters, unitOfWork.Transaction);
            if (result != null)
            {
                clientList = result.ToList();
            }

            int totalCount = parameters.Get<int>("@TotalCount");
            return Tuple.Create(totalCount, clientList);
        }

        public async Task<int?> CreateClientAsync(Client client)
        {
            string sql = $@"
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
			                    ,@AddressId);
                    set @ClientId = SCOPE_IDENTITY();  
            ";

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ClientName", client.ClientName, DbType.String, ParameterDirection.Input);
            parameters.Add("@IdentityCode", client.IdentityCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceRegisterNumber", client.InvoiceRegisterNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@VoucherIssuerId", client.VoucherIssuerId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BusinessTypeId", client.BusinessTypeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Status", client.Status, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SecurityAlgorithm", client.SecurityAlgorithm, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SecurityKey", client.SecurityKey, DbType.String, ParameterDirection.Input);
            parameters.Add("@NeedNotification", client.NeedNotification, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@NotificationProviderCodeId", client.NotificationProviderCodeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@LogoMediaId", client.LogoMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BannerMediaId", client.BannerMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailHeaderMediaId", client.EmailHeaderMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailFooterMediaId", client.EmailFooterMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CanIssue", client.CanIssue, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@MandatoryAutoBilling", client.MandatoryAutoBilling, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@InvoiceTitle", client.InvoiceTitle, DbType.String, ParameterDirection.Input);
            parameters.Add("@SubURL", client.SubURL, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailProviderCode", client.EmailProviderCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailSenderName", client.EmailSenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailSenderAddress", client.EmailSenderAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@ApplyEmailSubject", client.ApplyEmailSubject, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@SMSProviderCode", client.SMSProviderCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@SMSSenderName", client.SMSSenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@SmsEntityId", client.SmsEntityId, DbType.String, ParameterDirection.Input);
            parameters.Add("@SalesEmail", client.SalesEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactName", client.ContactName, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactEmail", client.ContactEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactPhone", client.ContactPhone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Memo", client.Memo, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", client.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@AddressId", client.AddressId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ClientId", 0, DbType.Int32, ParameterDirection.Output);

            await unitOfWork.Connection.ExecuteScalarAsync<int>(sql, parameters, unitOfWork.Transaction);
            int? ClientId = parameters.Get<int?>("@ClientId");
            return ClientId;
        }

        public async Task<int?> CreateBXPClientAsync(Client client)
        {
            string sql = @"
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
  
                        set @ClientId = SCOPE_IDENTITY(); 
                    ";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ClientName", client.ClientName, DbType.String, ParameterDirection.Input);
            parameters.Add("@IdentityCode", client.IdentityCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceRegisterNumber", client.InvoiceRegisterNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@SecurityKey", client.SecurityKey, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceTitle", client.InvoiceTitle, DbType.String, ParameterDirection.Input);
            parameters.Add("@AddressId", client.AddressId, DbType.String, ParameterDirection.Input);
            parameters.Add("@ClientId", 0, DbType.Int32, ParameterDirection.Output);
            await unitOfWork.Connection.ExecuteScalarAsync<int>(sql, parameters, unitOfWork.Transaction);
            int? ClientId = parameters.Get<int?>("@ClientId");
            return ClientId;
        }

        public async Task<int?> UpdateClientAsync(Client client)
        {
            string sql = $@"
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
            ";

            int? AddressId = 0;
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ClientId", client.ClientId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ClientName", client.ClientName, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceRegisterNumber", client.InvoiceRegisterNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@VoucherIssuerId", client.VoucherIssuerId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BusinessTypeId", client.BusinessTypeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Status", client.Status, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SecurityAlgorithm", client.SecurityAlgorithm, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SecurityKey", client.SecurityKey, DbType.String, ParameterDirection.Input);
            parameters.Add("@NeedNotification", client.NeedNotification, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@NotificationProviderCodeId", client.NotificationProviderCodeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@LogoMediaId", client.LogoMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BannerMediaId", client.BannerMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailHeaderMediaId", client.EmailHeaderMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailFooterMediaId", client.EmailFooterMediaId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CanIssue", client.CanIssue, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@MandatoryAutoBilling", client.MandatoryAutoBilling, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@InvoiceTitle", client.InvoiceTitle, DbType.String, ParameterDirection.Input);
            parameters.Add("@SubURL", client.SubURL, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailProviderCode", client.EmailProviderCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailSenderName", client.EmailSenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailSenderAddress", client.EmailSenderAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@ApplyEmailSubject", client.ApplyEmailSubject, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@SMSProviderCode", client.SMSProviderCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@SMSSenderName", client.SMSSenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@SmsEntityId", client.SmsEntityId, DbType.String, ParameterDirection.Input);
            parameters.Add("@SalesEmail", client.SalesEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactName", client.ContactName, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactEmail", client.ContactEmail, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactPhone", client.ContactPhone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Memo", client.Memo, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", client.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@AddressId", AddressId, DbType.Int32, ParameterDirection.Output);

            await unitOfWork.Connection.ExecuteAsync(sql, parameters, unitOfWork.Transaction);
            AddressId = parameters.Get<int?>("@AddressId");
            return AddressId;
        }

        public async Task DeleteClientById(int ClientId)
        {
            string sql = $@"Delete from [client].[tb_cbi_client_basic_information] where client_id = @ClientId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ClientId", ClientId, DbType.Int32, ParameterDirection.Input);

            await unitOfWork.Connection.ExecuteAsync(sql, parameters, unitOfWork.Transaction);
        }
    }
}

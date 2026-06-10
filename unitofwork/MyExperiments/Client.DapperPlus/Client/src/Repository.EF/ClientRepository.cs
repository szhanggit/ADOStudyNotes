using Core;
using Domain.Entities;
using Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetClientByIdAsync(int ClientId);
    }

    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ClientContext context) : base(context)
        {
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
                                c.[address_id] AS [AddressId],
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
            Client _client = new Client();

            var param = new SqlParameter[]
            {
                new SqlParameter("@ClientId", ClientId)
            };
            IQueryable<Client> result = null;
            result = await Task.FromResult(_context
            .Set<Client>()
            .FromSqlRaw(sql, param));

            if (result != null)
            {
                _client = result.AsEnumerable<Client>().FirstOrDefault();
            }

            return _client;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("tb_cbi_client_basic_information", Schema = "client")]
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }
        //[Column("client_name")]
        public string ClientName { get; set; }
        //[Column("identity_code")]
        public string IdentityCode { get; set; }
        //[Column("voucher_issuer_id")]
        public int? VoucherIssuerId { get; set; }
        //[Column("invoice_register_number")]
        public string InvoiceRegisterNumber { get; set; }
        //[Column("business_type_id")]
        public int? BusinessTypeId { get; set; }
        //[Column("status")]
        public byte Status { get; set; }
        //[Column("security_algorithm")]
        public byte SecurityAlgorithm { get; set; }
        //[Column("security_key")]
        public string SecurityKey { get; set; }
        //[Column("need_notification")]
        public bool NeedNotification { get; set; }
        //[Column("notification_provider_code_id")]
        public int? NotificationProviderCodeId { get; set; }
        //[Column("logo_media_id")]
        public int? LogoMediaId { get; set; }
        //[Column("banner_media_id")]
        public int? BannerMediaId { get; set; }
        //[Column("email_header_media_id")]
        public int? EmailHeaderMediaId { get; set; }
        //[Column("email_footer_media_id")]
        public int? EmailFooterMediaId { get; set; }
        //[Column("can_issue")]

        public bool CanIssue { get; set; }
        //[Column("mandatory_auto_billing")]
        public bool MandatoryAutoBilling { get; set; }
        //[Column("invoice_title")]
        public string InvoiceTitle { get; set; }
        //[Column("sub_url")]
        public string SubURL { get; set; }
        //[Column("email_provider_code")]
        public string EmailProviderCode { get; set; }
        //[Column("email_sender_name")]
        public string EmailSenderName { get; set; }
        //[Column("email_sender_address")]
        public string EmailSenderAddress { get; set; }
        //[Column("apply_email_subject")]
        public bool ApplyEmailSubject { get; set; }
        //[Column("sms_provider_code")]
        public string SMSProviderCode { get; set; }
        //[Column("sms_sender_name")]
        public string SMSSenderName { get; set; }
        //[Column("sms_entity_id")]
        public string SmsEntityId { get; set; }
        //[Column("sales_email")]
        public string SalesEmail { get; set; }
        //[Column("contact_name")]
        public string ContactName { get; set; }
        //[Column("contact_email")]
        public string ContactEmail { get; set; }
        //[Column("contact_phone")]
        public string ContactPhone { get; set; }
        public string Memo { get; set; }
        //[Column("description")]
        public string Description { get; set; }

        public int? AddressId { get; set; }
        public string DetailAddressLine { get; set; }
        public string District { get; set; }
        public int? CityId { get; set; }
        public int? StateOrProvinceId { get; set; }
        public string Postcode { get; set; }
        public int? CountryId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public byte AddressStatus { get; set; }
    }
}

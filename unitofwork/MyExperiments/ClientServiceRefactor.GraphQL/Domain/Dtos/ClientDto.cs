using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    [ExcludeFromCodeCoverageAttribute]
    public class ClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string IdentityCode { get; set; }
        public string InvoiceRegisterNumber { get; set; }
        public int VoucherIssuerId { get; set; }
        public int BusinessTypeId { get; set; }
        public int Status { get; set; }
        public int SecurityAlgorithm { get; set; }
        public string SecurityKey { get; set; }
        public bool NeedNotification { get; set; }
        public int NotificationProviderCodeId { get; set; }
        public int LogoMediaId { get; set; }
        public int BannerMediaId { get; set; }
        public int EmailHeaderMediaId { get; set; }
        public int EmailFooterMediaId { get; set; }
        public bool CanIssue { get; set; }
        public bool MandatoryAutoBilling { get; set; }
        public string InvoiceTitle { get; set; }
        public string SubURL { get; set; }
        public string EmailProviderCode { get; set; }
        public string EmailSenderName { get; set; }
        public string EmailSenderAddress { get; set; }
        public bool ApplyEmailSubject { get; set; }
        public string SMSProviderCode { get; set; }
        public string SMSSenderName { get; set; }
        public string SmsEntityId { get; set; }
        public string SalesEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Memo { get; set; }
        public string Description { get; set; }
        public string DetailAddressLine { get; set; }
        public string District { get; set; }
        public int? CityId { get; set; }
        public int? StateOrProvinceId { get; set; }
        public string Postcode { get; set; }
        public int? CountryId { get; set; }
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }
        public int AddressStatus { get; set; }
    }
}

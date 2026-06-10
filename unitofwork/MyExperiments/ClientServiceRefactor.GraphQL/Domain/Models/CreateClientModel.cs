using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [ExcludeFromCodeCoverageAttribute]
    public class CreateClientModel
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string ClientName { get; set; }
        public string InvoiceRegisterNumber { get; set; }
        public int? VoucherIssuerId { get; set; }
        public int? BusinessTypeId { get; set; }
        public int Status { get; set; }
        public int securityAlgorithm { get; set; }
        public string SecurityKey { get; set; }
        public bool needNotification { get; set; }
        public int? NotificationProviderCodeId { get; set; }
        public int? LogoMediaId { get; set; }
        public int? BannerMediaId { get; set; }
        public int? EmailHeaderMediaId { get; set; }
        public int? EmailFooterMediaId { get; set; }
        public bool canIssue { get; set; }
        public bool? MandatoryAutoBilling { get; set; }
        public string InvoiceTitle { get; set; }
        public string SubUrl { get; set; }
        public string EmailProviderCode { get; set; }
        public string EmailSenderName { get; set; }
        public string EmailSenderAddress { get; set; }
        public bool? ApplyEmailSubject { get; set; }
        public string SmsProviderCode { get; set; }
        public string SmsSenderName { get; set; }
        public string SmsEntityId { get; set; }
        public string SalesEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Memo { get; set; }
        public string Description { get; set; }
        public string TX2UserName { get; set; }
        public string DetailAddressLine { get; set; }
        public string District { get; set; }
        public int? CityId { get; set; }
        public int? StateOrProvinceId { get; set; }
        public string Postcode { get; set; }
        public int? CountryId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? AddressStatus { get; set; }
    }
}

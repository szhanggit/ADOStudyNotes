using System.ComponentModel.DataAnnotations;
using TXC.Common.Services.Wrappers;

namespace Services.Command.Client
{
    public class UpdateClientCommand : IRequestWrapper<int>
    {
        [Required]
        public int ClientId { get; set; }
        [MaxLength(100, ErrorMessage = "Client name shall be maximum 100 characters.")]
        public string ClientName { get; set; }
        [MaxLength(100, ErrorMessage = "Invoice Register Number shall be maximum 100 characters.")]
        public string InvoiceRegisterNumber { get; set; }
        public int? VoucherIssuerId { get; set; }
        public int? BusinessTypeId { get; set; }
        [Range(0, 1, ErrorMessage = "Client status can only be 0 or 1.")]
        public int Status { get; set; }
        public int SecurityAlgorithm { get; set; }
        public string SecurityKey { get; set; }
        public bool NeedNotification { get; set; }
        public int? NotificationProviderCodeId { get; set; }
        public int? LogoMediaId { get; set; }
        public int? BannerMediaId { get; set; }
        public int? EmailHeaderMediaId { get; set; }
        public int? EmailFooterMediaId { get; set; }
        public bool CanIssue { get; set; }
        public bool? MandatoryAutoBilling { get; set; }
        [MaxLength(100, ErrorMessage = "Invoice title shall be maximum 100 characters.")]
        public string InvoiceTitle { get; set; }
        [MaxLength(6, ErrorMessage = "SubRUL shall be maximum 6 characters.")]
        public string SubURL { get; set; }
        [MaxLength(8, ErrorMessage = "Email provider code shall be maximum 8 characters.")]
        public string EmailProviderCode { get; set; }
        [MaxLength(255, ErrorMessage = "Email sender name shall be maximum 225 characters.")]
        public string EmailSenderName { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid contact sender email address.")]
        [MaxLength(255, ErrorMessage = "Email sender address shall be maximum 225 characters.")]
        public string EmailSenderAddress { get; set; }
        public bool? ApplyEmailSubject { get; set; }
        [MaxLength(8, ErrorMessage = "Sms provider code shall be maximum 8 characters.")]
        public string SMSProviderCode { get; set; }
        [MaxLength(255, ErrorMessage = "Sms sender name shall be maximum 225 characters.")]
        public string SMSSenderName { get; set; }
        [MaxLength(30, ErrorMessage = "Sms entity id shall be maximum 30 characters.")]
        public string SmsEntityId { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid sales email.")]
        [MaxLength(255, ErrorMessage = "Sales email shall be maximum 225 characters.")]
        public string SalesEmail { get; set; }
        [MaxLength(30, ErrorMessage = "Contact name shall be maximum 30 characters.")]
        public string ContactName { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid contact email.")]
        [MaxLength(255, ErrorMessage = "Contact Email shall be maximum 225 characters.")]
        public string ContactEmail { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid contact phonenumber.")]
        [MaxLength(50, ErrorMessage = "Contact phone shall be maximum 50 characters.")]
        public string ContactPhone { get; set; }
        [MaxLength(2000, ErrorMessage = "Memo shall be maximum 2000 characters.")]
        public string Memo { get; set; }
        [MaxLength(500, ErrorMessage = "Description shall be maximum 500 characters.")]
        public string Description { get; set; }
        [MaxLength(400, ErrorMessage = "Detail address line shall be maximum 400 characters.")]
        public string DetailAddressLine { get; set; }
        [MaxLength(100, ErrorMessage = "District shall be maximum 100 characters.")]
        public string District { get; set; }
        public int CityId { get; set; }
        public int StateOrProvinceId { get; set; }
        [DataType(DataType.PostalCode, ErrorMessage = "Invalid postcode")]
        [MaxLength(20, ErrorMessage = "Postcode shall be maximum 20 characters.")]
        public string Postcode { get; set; }
        public int CountryId { get; set; }
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }
        [Range(0, 1, ErrorMessage = "AddressStatus can only be 0 or 1.")]
        public int AddressStatus { get; set; }
    }
}

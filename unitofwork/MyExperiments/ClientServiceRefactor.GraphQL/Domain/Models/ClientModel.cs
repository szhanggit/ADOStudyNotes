using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [ExcludeFromCodeCoverageAttribute]
    public class ClientModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Client name shall be maximum 100 characters.")]
        [Column("client_name", TypeName = "nvarchar(100)")]
        public string ClientName { get; set; }
        [Required]
        [Column("identity_code", TypeName = "varchar(50)")]
        public string IdentityCode { get; set; }
        [Column("Voucher_Issuer_Id", TypeName = "int")]
        public int? VoucherIssuerId { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Invoice Register Number shall be maximum 100 characters.")]
        [Column("invoice_register_number", TypeName = "nvarchar(100)")]
        public string InvoiceRegisterNumber { get; set; }
        [Column("Business_Type_Id", TypeName = "int")]
        public int? BusinessTypeId { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Client status can only be 0 or 1.")]
        [Column("Status", TypeName = "tinyint")]
        public byte Status { get; set; }
        [Required]
        [Column("Security_Algorithm", TypeName = "tinyint")]
        public byte SecurityAlgorithm { get; set; }
        [Required]
        [MaxLength(32)]
        [Column("security_key", TypeName = "varchar(32)")]
        public string SecurityKey { get; set; }
        [Required]
        [Column("Need_Notification", TypeName = "bit")]
        public bool NeedNotification { get; set; }
        [Column("Notification_Provider_Code_Id", TypeName = "int")]
        public int? NotificationProviderCodeId { get; set; }
        [Column("Logo_Media_Id", TypeName = "int")]
        public int? LogoMediaId { get; set; }
        [Column("Banner_Media_Id", TypeName = "int")]
        public int? BannerMediaId { get; set; }
        [Column("Email_Header_Media_Id", TypeName = "int")]
        public int? EmailHeaderMediaId { get; set; }
        [Column("Email_Footer_Media_Id", TypeName = "int")]
        public int? EmailFooterMediaId { get; set; }
        [Column("Can_Issue", TypeName = "bit")]
        [Required]
        public bool CanIssue { get; set; }
        [Column("Mandatory_Auto_Billing", TypeName = "bit")]
        public bool? MandatoryAutoBilling { get; set; }
        [Column("invoice_title", TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Invoice title shall be maximum 100 characters.")]
        public string InvoiceTitle { get; set; }
        [Column("Sub_URL", TypeName = "nvarchar(6)")]
        [MaxLength(6, ErrorMessage = "SubRUL shall be maximum 6 characters.")]
        public string SubURL { get; set; }
        [Column("Email_Provider_Code", TypeName = "nvarchar(8)")]
        [MaxLength(8, ErrorMessage = "Email provider code shall be maximum 8 characters.")]
        public string EmailProviderCode { get; set; }
        [Column("Email_Sender_Name", TypeName = "nvarchar(255)")]
        [MaxLength(255, ErrorMessage = "Email sender name shall be maximum 225 characters.")]
        public string EmailSenderName { get; set; }
        [Column("Email_Sender_Address", TypeName = "nvarchar(255)")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid contact sender email address.")]
        [MaxLength(255, ErrorMessage = "Email sender address shall be maximum 225 characters.")]
        public string EmailSenderAddress { get; set; }
        [Column("Apply_Email_Subject", TypeName = "bit")]
        public bool? ApplyEmailSubject { get; set; }
        [Column("SMS_Provider_Code", TypeName = "nvarchar(8)")]
        [MaxLength(8, ErrorMessage = "Sms provider code shall be maximum 8 characters.")]
        public string SMSProviderCode { get; set; }
        [Column("SMS_Sender_Name", TypeName = "nvarchar(255)")]
        [MaxLength(255, ErrorMessage = "Sms sender name shall be maximum 225 characters.")]
        public string SMSSenderName { get; set; }
        [Column("Sms_Entity_Id", TypeName = "nvarchar(30)")]
        [MaxLength(30, ErrorMessage = "Sms entity id shall be maximum 30 characters.")]
        public string SmsEntityId { get; set; }
        [Column("Sales_Email", TypeName = "nvarchar(255)")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid sales email.")]
        [MaxLength(255, ErrorMessage = "Sales email shall be maximum 225 characters.")]
        public string SalesEmail { get; set; }
        [Column("Contact_Name", TypeName = "nvarchar(30)")]
        [MaxLength(30, ErrorMessage = "Contact name shall be maximum 30 characters.")]
        public string ContactName { get; set; }
        [Column("Contact_Email", TypeName = "nvarchar(255)")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid contact email.")]
        [MaxLength(255, ErrorMessage = "Contact Email shall be maximum 225 characters.")]
        public string ContactEmail { get; set; }
        [Column("Contact_Phone", TypeName = "nvarchar(50)")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid contact phonenumber.")]
        [MaxLength(50, ErrorMessage = "Contact phone shall be maximum 50 characters.")]
        public string ContactPhone { get; set; }
        [Column("Memo", TypeName = "nvarchar(2000)")]
        [MaxLength(2000, ErrorMessage = "Memo shall be maximum 2000 characters.")]
        public string Memo { get; set; }
        [Column("Description", TypeName = "nvarchar(500)")]
        [MaxLength(500, ErrorMessage = "Description shall be maximum 500 characters.")]
        public string Description { get; set; }
        [Column("Address_Id", TypeName = "int")]
        public int? AddressId { get; set; }
        [Column("TimeStamp", TypeName = "timestamp")]
        public Byte[] TimeStamp { get; set; }
        public List<AddressModel> address { get; set; }
    }
}

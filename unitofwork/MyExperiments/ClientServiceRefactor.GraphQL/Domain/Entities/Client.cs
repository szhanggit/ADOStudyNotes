using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using d = Dapper.Contrib.Extensions;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverageAttribute]
    [d.Table("client.tb_cbi_client_basic_information")]
    public class Client
    {
        [d.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("client_id", TypeName = "int")]
        public int Client_Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Client name shall be maximum 100 characters.")]
        [Column("client_name", TypeName = "nvarchar(100)")]
        public string Client_Name { get; set; }
        [Required]
        [Column("identity_code", TypeName = "varchar(50)")]
        public string Identity_Code { get; set; }
        [Column("Voucher_Issuer_Id", TypeName = "int")]
        public int? Voucher_Issuer_Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Invoice Register Number shall be maximum 100 characters.")]
        [Column("invoice_register_number", TypeName = "nvarchar(100)")]
        public string Invoice_Register_Number { get; set; }
        [Column("Business_Type_Id", TypeName = "int")]
        public int? Business_Type_Id { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Client status can only be 0 or 1.")]
        [Column("Status", TypeName = "tinyint")]
        public byte Status { get; set; }
        [Required]
        [Column("Security_Algorithm", TypeName = "tinyint")]
        public byte Security_Algorithm { get; set; }
        [Required]
        [MaxLength(32)]
        [Column("security_key", TypeName = "varchar(32)")]
        public string Security_Key { get; set; }
        [Required]
        [Column("Need_Notification", TypeName = "bit")]
        public bool Need_Notification { get; set; }
        [Column("Notification_Provider_Code_Id", TypeName = "int")]
        public int? Notification_Provider_Code_Id { get; set; }
        [Column("Logo_Media_Id", TypeName = "int")]
        public int? Logo_Media_Id { get; set; }
        [Column("Banner_Media_Id", TypeName = "int")]
        public int? Banner_Media_Id { get; set; }
        [Column("Email_Header_Media_Id", TypeName = "int")]
        public int? Email_Header_Media_Id { get; set; }
        [Column("Email_Footer_Media_Id", TypeName = "int")]
        public int? Email_Footer_Media_Id { get; set; }
        [Column("Can_Issue", TypeName = "bit")]
        [Required]
        public bool Can_Issue { get; set; }
        [Column("Mandatory_Auto_Billing", TypeName = "bit")]
        public bool? Mandatory_Auto_Billing { get; set; }
        [Column("invoice_title", TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Invoice title shall be maximum 100 characters.")]
        public string Invoice_Title { get; set; }
        [Column("Sub_URL", TypeName = "nvarchar(6)")]
        [MaxLength(6, ErrorMessage = "SubRUL shall be maximum 6 characters.")]
        public string Sub_URL { get; set; }
        [Column("Email_Provider_Code", TypeName = "nvarchar(8)")]
        [MaxLength(8, ErrorMessage = "Email provider code shall be maximum 8 characters.")]
        public string Email_Provider_Code { get; set; }
        [Column("Email_Sender_Name", TypeName = "nvarchar(255)")]
        [MaxLength(255, ErrorMessage = "Email sender name shall be maximum 225 characters.")]
        public string Email_Sender_Name { get; set; }
        [Column("Email_Sender_Address", TypeName = "nvarchar(255)")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid contact sender email address.")]
        [MaxLength(255, ErrorMessage = "Email sender address shall be maximum 225 characters.")]
        public string Email_Sender_Address { get; set; }
        [Column("Apply_Email_Subject", TypeName = "bit")]
        public bool? Apply_Email_Subject { get; set; }
        [Column("SMS_Provider_Code", TypeName = "nvarchar(8)")]
        [MaxLength(8, ErrorMessage = "Sms provider code shall be maximum 8 characters.")]
        public string SMS_Provider_Code { get; set; }
        [Column("SMS_Sender_Name", TypeName = "nvarchar(255)")]
        [MaxLength(255, ErrorMessage = "Sms sender name shall be maximum 225 characters.")]
        public string SMS_Sender_Name { get; set; }
        [Column("Sms_Entity_Id", TypeName = "nvarchar(30)")]
        [MaxLength(30, ErrorMessage = "Sms entity id shall be maximum 30 characters.")]
        public string Sms_Entity_Id { get; set; }
        [Column("Sales_Email", TypeName = "nvarchar(255)")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid sales email.")]
        [MaxLength(255, ErrorMessage = "Sales email shall be maximum 225 characters.")]
        public string Sales_Email { get; set; }
        [Column("Contact_Name", TypeName = "nvarchar(30)")]
        [MaxLength(30, ErrorMessage = "Contact name shall be maximum 30 characters.")]
        public string Contact_Name { get; set; }
        [Column("Contact_Email", TypeName = "nvarchar(255)")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid contact email.")]
        [MaxLength(255, ErrorMessage = "Contact Email shall be maximum 225 characters.")]
        public string Contact_Email { get; set; }
        [Column("Contact_Phone", TypeName = "nvarchar(50)")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid contact phonenumber.")]
        [MaxLength(50, ErrorMessage = "Contact phone shall be maximum 50 characters.")]
        public string Contact_Phone { get; set; }
        [Column("Memo", TypeName = "nvarchar(2000)")]
        [MaxLength(2000, ErrorMessage = "Memo shall be maximum 2000 characters.")]
        public string Memo { get; set; }
        [Column("Description", TypeName = "nvarchar(500)")]
        [MaxLength(500, ErrorMessage = "Description shall be maximum 500 characters.")]
        public string Description { get; set; }
        [Column("Address_Id", TypeName = "int")]
        public int? Address_Id { get; set; }
        [d.Write(false)]
        [Column("TimeStamp", TypeName = "timestamp")]
        public Byte[] TimeStamp { get; set; }
    }
}

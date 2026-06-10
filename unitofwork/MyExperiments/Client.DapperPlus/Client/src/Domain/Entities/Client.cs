using System;
using d = Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Domain.Entities
{
    [d.Table("client.tb_cbi_client_basic_information")]
    public class Client
    {
        [d.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Client_Id { get; set; }
        public string Client_Name { get; set; }
        public string Identity_Code { get; set; }
        public int? Voucher_Issuer_Id { get; set; }
        public string Invoice_Register_Number { get; set; }
        public int? Business_Type_Id { get; set; }
        public byte Status { get; set; }
        public byte Security_Algorithm { get; set; }
        public string Security_Key { get; set; }
        public bool Need_Notification { get; set; }
        public int? Notification_Provider_Code_Id { get; set; }
        public int? Logo_Media_Id { get; set; }
        public int? Banner_Media_Id { get; set; }
        public int? Email_Header_Media_Id { get; set; }
        public int? Email_Footer_Media_Id { get; set; }
        public bool Can_Issue { get; set; }
        public bool? Mandatory_Auto_Billing { get; set; }
        public string Invoice_Title { get; set; }
        public string Sub_URL { get; set; }
        public string Email_Provider_Code { get; set; }
        public string Email_Sender_Name { get; set; }
        public string Email_Sender_Address { get; set; }
        public bool? Apply_Email_Subject { get; set; }
        public string SMS_Provider_Code { get; set; }
        public string SMS_Sender_Name { get; set; }       
        public string Sms_Entity_Id { get; set; }        
        public string Sales_Email { get; set; }
        public string Contact_Name { get; set; }
        public string Contact_Email { get; set; }
        public string Contact_Phone { get; set; }
        public string Memo { get; set; }
        public string Description { get; set; }
        public int? Address_Id { get; set; }
        [Write(false)]
        public Byte[] TimeStamp { get; set; }


    }
}

using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;


namespace Service.BusinessLogic
{
    public interface IObjectConvertingService
    {
        ClientMessageV1 ConvertUpdateClientRequestToClientMessageV1(UpdateClientRequest _updateClientRequest);
        ClientMessageV1 ConvertCreateClientRequestToClientMessageV1(CreateClientRequest _createClientRequest, int? ClientId, string ClientCode);
        ClientMessageV1 CreateBXPClientRequestToClientMessageV1(CreateBXPClientRequest _createClientRequest, int? ClientId, string ClientCode, string securityKey);
        ClientAddressModel CombineClientAddressIntoClientAddressModel(Domain.Entities.Client client, Address address);
        ClientListItem ConvertClientModelToClientListItem(Domain.Models.ClientModel _client);
        ClientListItem ConvertClientModelToClientListItemWithoutAddress(Domain.Models.ClientModel _client);
        ClientListItem ConvertClientEntityToClientListItem(Domain.Entities.Client _client);
        Domain.Entities.Client ConvertCreateClientRequestToClientEntity(CreateClientRequest request, string _identityCode);
        Domain.Entities.Address ConvertCreateClientRequestToAddressEntity(CreateClientRequest request);
        Domain.Entities.Client ConvertUpdateClientRequestToClientEntity(UpdateClientRequest request);
        Domain.Entities.Address ConvertUpdateClientRequestToAddressEntity(UpdateClientRequest request);
    }
    public class ObjectConvertingService : IObjectConvertingService
    {
        private readonly IMapper _mapper;
        public ObjectConvertingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ClientMessageV1 ConvertUpdateClientRequestToClientMessageV1(UpdateClientRequest _updateClientRequest)
        {
            ClientMessageV1 _result = new ClientMessageV1();
            if (_updateClientRequest == null)
            {
                return _result;
            }
            else
            {
                _result = _mapper.Map<ClientMessageV1>(_updateClientRequest);
                return _result;
            }
        }

        public ClientMessageV1 ConvertCreateClientRequestToClientMessageV1(CreateClientRequest _createClientRequest, int? ClientId, string ClientCode)
        {
            ClientMessageV1 _result = new ClientMessageV1();
            if (_createClientRequest == null || !ClientId.HasValue)
            {
                return _result;
            }
            else
            {
                _result = _mapper.Map<ClientMessageV1>(_createClientRequest);
                _result.ClientId = ClientId ?? 0;
                _result.IdentityCode = ClientCode;
                return _result;
            }
        }

        public ClientMessageV1 CreateBXPClientRequestToClientMessageV1(CreateBXPClientRequest _createClientRequest, int? ClientId, string ClientCode, string securityKey)
        {
            ClientMessageV1 _result = new ClientMessageV1();
            if (_createClientRequest == null || !ClientId.HasValue)
            {
                return _result;
            }
            else
            {
                _result.InvoiceTitle = _createClientRequest.InvoiceTitle;
                _result.SecurityKey = securityKey;
                _result.SecurityAlgorithm = (int)SecurityAlgorithmLength.DES;
                _result.Status = 1;
                _result.InvoiceRegisterNumber = _createClientRequest.InvoiceRegisterNumber;
                _result.IdentityCode = ClientCode;
                _result.ClientName = _createClientRequest.ClientName;
                _result.ClientId = ClientId ?? 0;
                _result.DetailAddressLine = _createClientRequest.DetailAddressLine;
                _result.District = _createClientRequest.District;
                _result.CityId = _createClientRequest.CityId;
                _result.StateOrProvinceId = _createClientRequest.StateOrProvinceId;
                _result.Postcode = _createClientRequest.Postcode;
                _result.CountryId = _createClientRequest.CountryId;
                _result.Longitude = _createClientRequest.Longitude;
                _result.Latitude = _createClientRequest.Latitude;
                _result.AddressStatus = 1;
                return _result;
            }
        }

        public ClientAddressModel CombineClientAddressIntoClientAddressModel(Domain.Entities.Client client, Address address)
        {
            ClientAddressModel _clientAddressModel = null;
            if (client != null)
            {
                _clientAddressModel = new ClientAddressModel
                {
                    ClientId = client.Client_Id,
                    ClientName = client.Client_Name,
                    IdentityCode = client.Identity_Code,
                    VoucherIssuerId = client.Voucher_Issuer_Id,
                    InvoiceRegisterNumber = client.Invoice_Register_Number,
                    BusinessTypeId = client.Business_Type_Id,
                    Status = client.Status,
                    SecurityAlgorithm = client.Security_Algorithm,
                    SecurityKey = client.Security_Key,
                    NeedNotification = client.Need_Notification,
                    NotificationProviderCodeId = client.Notification_Provider_Code_Id,
                    LogoMediaId = client.Logo_Media_Id,
                    BannerMediaId = client.Banner_Media_Id,
                    EmailHeaderMediaId = client.Email_Header_Media_Id,
                    EmailFooterMediaId = client.Email_Footer_Media_Id,
                    CanIssue = client.Can_Issue,
                    MandatoryAutoBilling = client.Mandatory_Auto_Billing,
                    InvoiceTitle = client.Invoice_Title,
                    SubURL = client.Sub_URL,
                    EmailProviderCode = client.Email_Provider_Code,
                    EmailSenderName = client.Email_Sender_Name,
                    EmailSenderAddress = client.Email_Sender_Address,
                    ApplyEmailSubject = client.Apply_Email_Subject,
                    SMSProviderCode = client.SMS_Provider_Code,
                    SMSSenderName = client.SMS_Sender_Name,
                    SmsEntityId = client.Sms_Entity_Id,
                    SalesEmail = client.Sales_Email,
                    ContactName = client.Contact_Name,
                    ContactEmail = client.Contact_Email,
                    ContactPhone = client.Contact_Phone,
                    Memo = client.Memo,
                    Description = client.Description,

                };

                if (address != null)
                {
                    _clientAddressModel.AddressId = client.Address_Id;
                    _clientAddressModel.DetailAddressLine = address.Detail_Address_Line;
                    _clientAddressModel.District = address.District;
                    _clientAddressModel.CityId = address.City_Id;
                    _clientAddressModel.StateOrProvinceId = address.State_Province_Id;
                    _clientAddressModel.Postcode = address.PostCode;
                    _clientAddressModel.CountryId = address.Country_Id;
                    _clientAddressModel.Longitude = address.Longitude;
                    _clientAddressModel.Latitude = address.Latitude;
                    _clientAddressModel.AddressStatus = address.Status;
                }
            }

            return _clientAddressModel;
        }

        public ClientListItem ConvertClientModelToClientListItem(Domain.Models.ClientModel _client)
        {
            ClientListItem ClientItem = null;
            ClientItem = new ClientListItem
            {
                SalesEmail = _client?.SalesEmail,
                SecurityAlgorithm = _client.SecurityAlgorithm,
                SecurityKey = _client.SecurityKey,
                SmsEntityId = _client.SmsEntityId,
                Description = _client.Description,
                SmsProviderCode = _client.SMSProviderCode,
                SmsSenderName = _client.SMSSenderName,
                Status = _client.Status,
                SubUrl = _client.SubURL,
                ApplyEmailSubject = _client.ApplyEmailSubject ?? false,
                EmailSenderAddress = _client.EmailSenderAddress,
                EmailSenderName = _client.EmailSenderName,
                BannerMediaId = _client.BannerMediaId,
                BusinessTypeId = _client.BusinessTypeId,
                CanIssue = _client.CanIssue,
                MandatoryAutoBilling = _client.MandatoryAutoBilling ?? false,
                ClientId = _client.Id,
                ClientName = _client.ClientName,
                ContactEmail = _client.ContactEmail,
                ContactName = _client.ContactName,
                ContactPhone = _client.ContactPhone,
                EmailFooterMediaId = _client.EmailFooterMediaId,
                EmailHeaderMediaId = _client.EmailHeaderMediaId,
                EmailProviderCode = _client.EmailProviderCode,
                IdentityCode = _client.IdentityCode,
                InvoiceRegisterNumber = _client.InvoiceRegisterNumber,
                InvoiceTitle = _client.InvoiceTitle,
                LogoMediaId = _client.LogoMediaId,
                Memo = _client.Memo,
                NeedNotification = _client.NeedNotification,
                NotificationProviderCodeId = _client.NotificationProviderCodeId,
                VoucherIssuerId = _client.VoucherIssuerId,
                StateOrProvinceId = _client.address?.FirstOrDefault()?.ProvinceId ?? 0,
                AddressStatus = _client.address?.FirstOrDefault()?.Status ?? 0,
                CityId = _client.address?.FirstOrDefault()?.CityId ?? 0,
                CountryId = _client.address?.FirstOrDefault()?.CountryId ?? 0,
                DetailAddressLine = _client.address?.FirstOrDefault()?.DetailAddressLine,
                District = _client.address?.FirstOrDefault()?.District,
                Latitude = (float)(_client.address?.FirstOrDefault()?.Latitude ?? 0),
                Longitude = (float)(_client.address?.FirstOrDefault()?.Longitude ?? 0),
                Postcode = _client.address?.FirstOrDefault()?.PostCode,
            };
            return ClientItem;
        }

        public ClientListItem ConvertClientModelToClientListItemWithoutAddress(Domain.Models.ClientModel _client)
        {
            ClientListItem client = null;
            client = new ClientListItem
            {
                SalesEmail = _client.SalesEmail,
                SecurityAlgorithm = _client.SecurityAlgorithm,
                SecurityKey = _client.SecurityKey,
                SmsEntityId = _client.SmsEntityId,
                Description = _client.Description,
                SmsProviderCode = _client.SMSProviderCode,
                SmsSenderName = _client.SMSSenderName,
                Status = _client.Status,
                SubUrl = _client.SubURL,
                ApplyEmailSubject = _client.ApplyEmailSubject ?? false,
                EmailSenderAddress = _client.EmailSenderAddress,
                EmailSenderName = _client.EmailSenderName,
                BannerMediaId = _client.BannerMediaId,
                BusinessTypeId = _client.BusinessTypeId,
                CanIssue = _client.CanIssue,
                MandatoryAutoBilling = _client.MandatoryAutoBilling ?? false,
                ClientId = _client.Id,
                ClientName = _client.ClientName,
                ContactEmail = _client.ContactEmail,
                ContactName = _client.ContactName,
                ContactPhone = _client.ContactPhone,
                EmailFooterMediaId = _client.EmailFooterMediaId,
                EmailHeaderMediaId = _client.EmailHeaderMediaId,
                EmailProviderCode = _client.EmailProviderCode,
                IdentityCode = _client.IdentityCode,
                InvoiceRegisterNumber = _client.InvoiceRegisterNumber,
                InvoiceTitle = _client.InvoiceTitle,
                LogoMediaId = _client.LogoMediaId,
                Memo = _client.Memo,
                NeedNotification = _client.NeedNotification,
                NotificationProviderCodeId = _client.NotificationProviderCodeId,
                VoucherIssuerId = _client.VoucherIssuerId
            };
            return client;
        }

        public ClientListItem ConvertClientEntityToClientListItem(Domain.Entities.Client _client)
        {
            ClientListItem ClientItem = null;
            ClientItem = new ClientListItem
            {
                SalesEmail = _client?.Sales_Email,
                SecurityAlgorithm = _client.Security_Algorithm,
                SecurityKey = _client.Security_Key,
                SmsEntityId = _client.Sms_Entity_Id,
                Description = _client.Description,
                SmsProviderCode = _client.SMS_Provider_Code,
                SmsSenderName = _client.SMS_Sender_Name,
                Status = _client.Status,
                SubUrl = _client.Sub_URL,
                ApplyEmailSubject = _client.Apply_Email_Subject ?? false,
                EmailSenderAddress = _client.Email_Sender_Address,
                EmailSenderName = _client.Email_Sender_Name,
                BannerMediaId = _client.Banner_Media_Id,
                BusinessTypeId = _client.Business_Type_Id,
                CanIssue = _client.Can_Issue,
                MandatoryAutoBilling = _client.Mandatory_Auto_Billing ?? false,
                ClientId = _client.Client_Id,
                ClientName = _client.Client_Name,
                ContactEmail = _client.Contact_Email,
                ContactName = _client.Contact_Name,
                ContactPhone = _client.Contact_Phone,
                EmailFooterMediaId = _client.Email_Footer_Media_Id,
                EmailHeaderMediaId = _client.Email_Header_Media_Id,
                EmailProviderCode = _client.Email_Provider_Code,
                IdentityCode = _client.Identity_Code,
                InvoiceRegisterNumber = _client.Invoice_Register_Number,
                InvoiceTitle = _client.Invoice_Title,
                LogoMediaId = _client.Logo_Media_Id,
                Memo = _client.Memo,
                NeedNotification = _client.Need_Notification,
                NotificationProviderCodeId = _client.Notification_Provider_Code_Id,
                VoucherIssuerId = _client.Voucher_Issuer_Id
            };

            return ClientItem;
        }

        public Domain.Entities.Client ConvertCreateClientRequestToClientEntity(CreateClientRequest request, string _identityCode)
        {
            Domain.Entities.Client client = new Domain.Entities.Client
            {
                Sales_Email = request.SalesEmail,
                Security_Algorithm = (byte)request.SecurityAlgorithm,
                Security_Key = request.SecurityKey,
                Sms_Entity_Id = request.SmsEntityId,
                SMS_Provider_Code = request.SmsProviderCode,
                SMS_Sender_Name = request.SmsSenderName,
                Status = (byte)request.Status,
                Description = request.Description,
                Sub_URL = request.SubUrl,
                Apply_Email_Subject = request.ApplyEmailSubject ?? false,
                Email_Sender_Address = request.EmailSenderAddress,
                Email_Sender_Name = request.EmailSenderName,
                Banner_Media_Id = request.BannerMediaId,
                Business_Type_Id = request.BusinessTypeId,
                Can_Issue = request.CanIssue,
                Client_Name = request.ClientName,
                Contact_Email = request.ContactEmail,
                Contact_Name = request.ContactName,
                Contact_Phone = request.ContactPhone,
                Email_Footer_Media_Id = request.EmailFooterMediaId,
                Email_Header_Media_Id = request.EmailHeaderMediaId,
                Email_Provider_Code = request.EmailProviderCode,
                Invoice_Register_Number = request.InvoiceRegisterNumber,
                Invoice_Title = request.InvoiceTitle,
                Logo_Media_Id = request.LogoMediaId,
                Mandatory_Auto_Billing = request.MandatoryAutoBilling ?? false,
                Memo = request.Memo,
                Need_Notification = request.NeedNotification,
                Notification_Provider_Code_Id = request.NotificationProviderCodeId,
                Voucher_Issuer_Id = request.VoucherIssuerId,
                Identity_Code = _identityCode
            };

            return client;
        }

        public Domain.Entities.Address ConvertCreateClientRequestToAddressEntity(CreateClientRequest request)
        {
            Address address = new Address
            {
                State_Province_Id = request.StateOrProvinceId,
                City_Id = request.CityId,
                Country_Id = request.CountryId,
                Status = request.AddressStatus ?? 0,
                Detail_Address_Line = request.DetailAddressLine,
                District = request.District,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                PostCode = request.Postcode,
            };

            return address;
        }

        public Domain.Entities.Client ConvertUpdateClientRequestToClientEntity(UpdateClientRequest request)
        {
            Domain.Entities.Client client = new Domain.Entities.Client
            {
                Sales_Email = request.SalesEmail,
                Security_Algorithm = (byte)request.SecurityAlgorithm,
                Identity_Code = request.IdentityCode,
                Security_Key = request.SecurityKey,
                Sms_Entity_Id = request.SmsEntityId,
                SMS_Provider_Code = request.SmsProviderCode,
                SMS_Sender_Name = request.SmsSenderName,
                Status = (byte)request.Status,
                Description = request.Description,
                Sub_URL = request.SubUrl,
                Apply_Email_Subject = request.ApplyEmailSubject ?? false,
                Email_Sender_Address = request.EmailSenderAddress,
                Email_Sender_Name = request.EmailSenderName,
                Banner_Media_Id = request.BannerMediaId,
                Business_Type_Id = request.BusinessTypeId,
                Can_Issue = request.CanIssue,
                Client_Name = request.ClientName,
                Contact_Email = request.ContactEmail,
                Contact_Name = request.ContactName,
                Contact_Phone = request.ContactPhone,
                Email_Footer_Media_Id = request.EmailFooterMediaId,
                Email_Header_Media_Id = request.EmailHeaderMediaId,
                Email_Provider_Code = request.EmailProviderCode,
                Invoice_Register_Number = request.InvoiceRegisterNumber,
                Invoice_Title = request.InvoiceTitle,
                Logo_Media_Id = request.LogoMediaId,
                Mandatory_Auto_Billing = request.MandatoryAutoBilling ?? false,
                Memo = request.Memo,
                Need_Notification = request.NeedNotification,
                Notification_Provider_Code_Id = request.NotificationProviderCodeId,
                Voucher_Issuer_Id = request.VoucherIssuerId,
            };

            return client;
        }

        public Domain.Entities.Address ConvertUpdateClientRequestToAddressEntity(UpdateClientRequest request)
        {
            Domain.Entities.Address address = new Domain.Entities.Address
            {
                State_Province_Id = request.StateOrProvinceId,
                Status = (byte)request.AddressStatus,
                District = request.District,
                Country_Id = request.CountryId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                PostCode = request.Postcode,
                Detail_Address_Line = request.DetailAddressLine,
                City_Id = request.CityId,
            };

            return address;
        }
    }
}

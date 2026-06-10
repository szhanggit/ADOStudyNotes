using AutoMapper;
using Domain.Entities;
using Domain.Models;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;
using static Domain.Enums.Enums;

namespace Service.BusinessLogic
{
    public interface IObjectConvertingService
    {
        ClientMessageV1 ConvertUpdateClientRequestToClientMessageV1(UpdateClientRequest _updateClientRequest);
        ClientMessageV1 ConvertCreateClientRequestToClientMessageV1(CreateClientRequest _createClientRequest, int? ClientId, string ClientCode);
        ClientMessageV1 CreateBXPClientRequestToClientMessageV1(CreateBXPClientRequest _createClientRequest, int? ClientId, string ClientCode, string securityKey);
        ClientAddressModel CombineClientAddressIntoClientAddressModel(Domain.Entities.Client client, Address address);
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
                    _clientAddressModel.Postcode = address.Postcode;
                    _clientAddressModel.CountryId = address.Country_Id;
                    _clientAddressModel.Longitude = address.Longitude;
                    _clientAddressModel.Latitude = address.Latitude;
                    _clientAddressModel.AddressStatus = address.Status;
                }
            }

            return _clientAddressModel;
        }
    }
}

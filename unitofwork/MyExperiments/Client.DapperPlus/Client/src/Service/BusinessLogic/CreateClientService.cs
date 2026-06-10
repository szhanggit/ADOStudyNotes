using Domain.Models;
using Repository;
using System.Data;
using TXC.Proto.Credit;
using TXC.Proto.Client;
using Domain.Entities;

namespace Service.BusinessLogic
{
    public delegate Task<Tuple<bool, string>> CheckAddressByCityDel(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection);
    public interface ICreateClientService
    {
        public Task<CreateClientResponse> CreateClient(CreateClientRequest request);
    }

    public class CreateClientService : ICreateClientService
    {
        private IDbConnection _dbConnection;
        private readonly IClientDBService _clientDBService;
        private readonly ICoreService _coreService;
        private readonly ICommonClientService _commonClientService;
        private readonly IObjectConvertingService _objectConvertingService;

        public CreateClientService(
            IClientDBService clientDBService,
            ICoreService coreService,
            ICommonClientService commonClientService,
            IObjectConvertingService objectConvertingService)
        {
            _clientDBService = clientDBService;
            _coreService = coreService;
            _commonClientService = commonClientService;
            _objectConvertingService = objectConvertingService;
        }

        public async Task<CreateClientResponse> CreateClient(CreateClientRequest request)
        {
            try
            {
                if (request.TenantId <= 0)
                    return new CreateClientResponse() { Success = false, Message = "TenantBasicInfoId header required" };

                if (string.IsNullOrEmpty(request.TenantName))
                    return new CreateClientResponse() { Success = false, Message = "TenantName header required" };

                // initialize db connection
                var conn = await _coreService.GetDBConnection(request.TenantId);

                if (!conn.Success)
                    return new CreateClientResponse() { Success = false, Message = "Error in Tenant DB" };

                _dbConnection = conn.Data;

                //check tx2 connector config
                var queueNameConfig = await _coreService.GetConfig("TX2ConnectorQueueName", request.TenantId);
                string identityCode = await _clientDBService.GenerateClientIdentityAsync(request.TenantId, _dbConnection);

                if (string.IsNullOrWhiteSpace(identityCode))
                {
                    return new CreateClientResponse() { Success = false, Message = "Failed to generate client identity code" };
                }

                Tuple<bool, string> result = await _clientDBService.CheckIfValidAddressAsync(request.CityId, request.StateOrProvinceId, request.CountryId, _dbConnection);

                if (result.Item1 == false && request.CountryId.HasValue)
                {
                    return new CreateClientResponse() { Success = false, Message = result.Item2 };
                }


                Domain.Entities.Client client = new Domain.Entities.Client { 
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
                    Identity_Code = identityCode
                };

                Address address = new Address { 
                    State_Province_Id = request.StateOrProvinceId,
                    City_Id = request.CityId,
                    Country_Id = request.CountryId,
                    Status = request.AddressStatus,
                    Detail_Address_Line = request.DetailAddressLine,
                    District = request.District,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Postcode = request.Postcode,
                };

                int? ClientId = await _clientDBService.InsertClientAsync(client, address, _dbConnection);
                if (!ClientId.HasValue)
                {
                    return new CreateClientResponse() { Success = false, Message = "Failed to create new client" };
                }
                CreateClientWalletRequest createClientWalletRequest = new CreateClientWalletRequest()
                {
                    TenantId = request.TenantId,
                    ClientId = (int)ClientId,
                    AccountName = "default",
                    TenantName = request.TenantName
                };

                TXC.Proto.Credit.ProtoBaseResponse protoBaseResponseWallet = await _commonClientService.CreateClientWalletAsync(createClientWalletRequest);
                if (!protoBaseResponseWallet.Success)
                {
                    await _clientDBService.DeleteClientByIdAsync(ClientId.Value, _dbConnection);
                    return new CreateClientResponse() { Success = false, Message = "Failed to create new wallet" };
                }

                var message = _objectConvertingService.ConvertCreateClientRequestToClientMessageV1(request, ClientId, identityCode);

                //send to service bus
                bool _sendingResult = await _commonClientService.SendCreateMessageAsync(request.TenantId, queueNameConfig.Value, message);
                if (_sendingResult)
                {
                    return new CreateClientResponse { Success = true, Message = "Success", Data = ClientId ?? 0 };
                }
                else
                {
                    return new CreateClientResponse() { Success = false, Message = "Fail to be sent to service bus", Data = ClientId ?? 0 };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

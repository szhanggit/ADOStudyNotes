using Repository;
using Repository.Dapper;
using System.Data;
using TXC.Proto.Client;

namespace Service.BusinessLogic
{
    public interface IUpdateClientService
    {
        public Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request);
    }
    public class UpdateClientService : IUpdateClientService
    {
        private IDbConnection _dbConnection;
        private readonly IClientDBService _clientDBService;
        private readonly ICoreService _coreService;
        private readonly ICommonClientService _commonClientService;
        private readonly IObjectConvertingService _objectConvertingService;

        public UpdateClientService(IClientDBService clientDBService,
                                   ICoreService coreService,
                                   ICommonClientService commonClientService,
                                   IObjectConvertingService objectConvertingService)
        {
            _clientDBService = clientDBService;
            _coreService = coreService;
            _commonClientService = commonClientService;
            _objectConvertingService = objectConvertingService;
        }

        public async Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request)
        {
            Domain.Entities.Client client = null;
            Domain.Entities.Address address = null;

            try
            {
                if (request.TenantId <= 0)
                    return new UpdateClientResponse() { Success = false, Message = "TenantBasicInfoId header required" };

                if (string.IsNullOrEmpty(request.TenantName))
                    return new UpdateClientResponse() { Success = false, Message = "TenantName header required" };

                if (string.IsNullOrEmpty(request.IdentityCode) && request.ClientId <= 0)
                    return new UpdateClientResponse() { Success = false, Message = "Invalid Request" };

                // initialize db connection
                var conn = await _coreService.GetDBConnection(request.TenantId);

                if (!conn.Success)
                    return new UpdateClientResponse() { Success = false, Message = "Error in Tenant DB" };

                _dbConnection = conn.Data;

                //check tx2 connector config
                var queueNameConfig = await _coreService.GetConfig("TX2ConnectorQueueName", request.TenantId);

                int RowCount = await _clientDBService.CheckClientIdAsync(request.ClientId, _dbConnection);
                if (RowCount != 1)
                {
                    return new UpdateClientResponse() { Success = false, Message = "The client does not exist." };
                }

                if (request.CountryId.HasValue)
                {
                    Tuple<bool, string> result = await _clientDBService.CheckIfValidAddressAsync(request.CityId, request.StateOrProvinceId, request.CountryId, _dbConnection);

                    if (result.Item1 == false && request.CountryId.HasValue)
                    {
                        return new UpdateClientResponse() { Success = false, Message = result.Item2 };
                    }
                }

                client = new Domain.Entities.Client
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

                if (request.CountryId.HasValue)
                {
                    address = new Domain.Entities.Address
                    {
                        State_Province_Id = request.StateOrProvinceId,
                        Status = (byte)request.AddressStatus,
                        District = request.District,
                        Country_Id = request.CountryId,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        Postcode = request.Postcode,
                        Detail_Address_Line = request.DetailAddressLine,
                        City_Id = request.CityId,
                    };
                }

                bool _updateSuccess = await _clientDBService.UpdateClientAsync(client, address, _dbConnection);
                if (!_updateSuccess)
                    return new UpdateClientResponse() { Success = false, Message = "Failed to update new client", Data = 0 };

                var message = _objectConvertingService.ConvertUpdateClientRequestToClientMessageV1(request);

                //send to service bus
                bool _sendingResult = await _commonClientService.SendUpdateMessageAsync(request.TenantId, queueNameConfig.Value, message);
                if (_sendingResult)
                {
                    return new UpdateClientResponse() { Success = true, Message = "Success", Data = request.ClientId };
                }
                else
                {
                    return new UpdateClientResponse() { Success = false, Message = "Fail to be sent to service bus", Data = request.ClientId };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

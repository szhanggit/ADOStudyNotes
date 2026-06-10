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
        private readonly IClientOperation _clientOperation;
        private readonly ICoreService _coreService;
        private readonly ICommonClientService _commonClientService;
        private readonly IObjectConvertingService _objectConvertingService;

        public UpdateClientService(IClientOperation clientOperation,
                                   ICoreService coreService,
                                   ICommonClientService commonClientService,
                                   IObjectConvertingService objectConvertingService)
        {
            _clientOperation = clientOperation;
            _coreService = coreService;
            _commonClientService = commonClientService;
            _objectConvertingService = objectConvertingService;
        }

        public async Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request)
        {
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

                int RowCount = await _clientOperation.CheckClientIdAsync(request.ClientId, _dbConnection);
                if (RowCount != 1)
                {
                    return new UpdateClientResponse() { Success = false, Message = "The client does not exist." };
                }

                Tuple<bool, string> result = await _clientOperation.CheckIfValidAddressAsync(request.CityId, request.StateOrProvinceId, request.CountryId, _dbConnection);

                if (result.Item1 == false && request.CountryId.HasValue)
                {
                    return new UpdateClientResponse() { Success = false, Message = result.Item2 };
                }

                Domain.Entities.Client client = new Domain.Entities.Client
                {
                    SalesEmail = request.SalesEmail,
                    SecurityAlgorithm = (byte)request.SecurityAlgorithm,
                    SecurityKey = request.SecurityKey,
                    SmsEntityId = request.SmsEntityId,
                    SMSProviderCode = request.SmsProviderCode,
                    SMSSenderName = request.SmsSenderName,
                    StateOrProvinceId = request.StateOrProvinceId,
                    Status = (byte)request.Status,
                    Description = request.Description,
                    SubURL = request.SubUrl,
                    AddressStatus = (byte)request.AddressStatus,
                    ApplyEmailSubject = request.ApplyEmailSubject ?? false,
                    DetailAddressLine = request.DetailAddressLine,
                    EmailSenderAddress = request.EmailSenderAddress,
                    EmailSenderName = request.EmailSenderName,
                    District = request.District,
                    BannerMediaId = request.BannerMediaId,
                    BusinessTypeId = request.BusinessTypeId,
                    CanIssue = request.CanIssue,
                    CityId = request.CityId,
                    ClientName = request.ClientName,
                    ContactEmail = request.ContactEmail,
                    ContactName = request.ContactName,
                    ContactPhone = request.ContactPhone,
                    CountryId = request.CountryId,
                    EmailFooterMediaId = request.EmailFooterMediaId,
                    EmailHeaderMediaId = request.EmailHeaderMediaId,
                    EmailProviderCode = request.EmailProviderCode,
                    InvoiceRegisterNumber = request.InvoiceRegisterNumber,
                    InvoiceTitle = request.InvoiceTitle,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    LogoMediaId = request.LogoMediaId,
                    MandatoryAutoBilling = request.MandatoryAutoBilling ?? false,
                    Memo = request.Memo,
                    NeedNotification = request.NeedNotification,
                    NotificationProviderCodeId = request.NotificationProviderCodeId,
                    Postcode = request.Postcode,
                    VoucherIssuerId = request.VoucherIssuerId,
                };


                int? dbaffectedRows = await _clientOperation.UpdateClientAsync(client, _dbConnection);
                if (!dbaffectedRows.HasValue || dbaffectedRows < 1)
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

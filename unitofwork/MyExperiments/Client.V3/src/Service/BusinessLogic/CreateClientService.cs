using Domain.Models;
using Repository.Dapper;
using System.Data;
using TXC.Proto.Credit;
using TXC.Proto.Client;

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
        private readonly IClientOperation _clientOperation;
        private readonly ICoreService _coreService;
        private readonly ICommonClientService _commonClientService;
        private readonly IObjectConvertingService _objectConvertingService;

        public CreateClientService(
            IClientOperation clientOperation,
            ICoreService coreService,
            ICommonClientService commonClientService,
            IObjectConvertingService objectConvertingService)
        {
            _clientOperation = clientOperation;
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

                GenerateClientIdentityCodeModel generateClientIdentityCode = new GenerateClientIdentityCodeModel
                {
                    SequenceName = "client.seq_client_identity_code",
                    IsFixReturnLength = true,
                    ReturnLength = 20,
                    PaddingCharacter = '0',
                    TenantId = request.TenantId,
                };

                Tuple<bool, string> result = await _clientOperation.CheckIfValidAddressAsync(request.CityId, request.StateOrProvinceId, request.CountryId, _dbConnection);

                if (result.Item1 == false && request.CountryId.HasValue)
                {
                    return new CreateClientResponse() { Success = false, Message = result.Item2 };
                }


                Domain.Entities.Client client = new Domain.Entities.Client { 
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
                    AddressStatus = (byte)(request.AddressStatus ?? 0),
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
                Tuple<int?, string> _insertedResult = await _clientOperation.InsertClientAsync(client, generateClientIdentityCode, _dbConnection);
                int? ClientId = _insertedResult.Item1;
                string identityCode = _insertedResult.Item2;

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
                    await _clientOperation.DeleteClientByIdAsync(ClientId.Value, _dbConnection);
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

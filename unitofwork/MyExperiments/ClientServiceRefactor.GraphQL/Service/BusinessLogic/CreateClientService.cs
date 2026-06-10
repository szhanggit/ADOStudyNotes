using Domain.Models;
using Repository;
using System.Data;
using TXC.Proto.Credit;
using TXC.Proto.Client;
using Domain.Entities;
using Domain.Enums;
using Service.Utility;
using FluentValidation;

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
        private readonly IGetDictionaryListGraphQLService _getDictionaryListGraphQLService;
        private readonly IClientFetchingGraphQLService _clientFetchingGraphQLService;
        private readonly ISecurityKeyService _securityKeyService;
        private readonly IValidator<CreateClientRequest> _validator;
        private string _identityCode = string.Empty;
        private string _securityKey = string.Empty;

        public CreateClientService(
            IClientDBService clientDBService,
            ICoreService coreService,
            ICommonClientService commonClientService,
            IObjectConvertingService objectConvertingService,
            IGetDictionaryListGraphQLService getDictionaryListGraphQLService,
            IClientFetchingGraphQLService clientFetchingGraphQLService,
            ISecurityKeyService securityKeyService,
            IValidator<CreateClientRequest> validator)
        {
            _clientDBService = clientDBService;
            _coreService = coreService;
            _commonClientService = commonClientService;
            _objectConvertingService = objectConvertingService;
            _getDictionaryListGraphQLService = getDictionaryListGraphQLService;
            _clientFetchingGraphQLService = clientFetchingGraphQLService;
            _securityKeyService = securityKeyService;
            _validator = validator;
        }

        public async Task<CreateClientResponse> CreateClient(CreateClientRequest request)
        {
            try
            {
                if (request.TenantId <= 0)
                    return new CreateClientResponse() { Success = false, Message = "TenantBasicInfoId header required" };

                if (string.IsNullOrEmpty(request.TenantName))
                    return new CreateClientResponse() { Success = false, Message = "TenantName header required" };

                var vresult = await _validator.ValidateAsync(request);

                if (!vresult.IsValid)
                {
                    string _errorMessage = vresult.Errors.FirstOrDefault().ErrorMessage;
                    return new CreateClientResponse() { Success = false, Message = _errorMessage };
                }

                // initialize db connection
                var conn = await _coreService.GetDBConnection(request.TenantId);

                if (!conn.Success)
                    return new CreateClientResponse() { Success = false, Message = "Error in Tenant DB" };

                _dbConnection = conn.Data;

                //check tx2 connector config
                var queueNameConfig = await _coreService.GetConfig("TX2ConnectorQueueName", request.TenantId);
                GetClientListModel _getClientListModel = new GetClientListModel { TenantId = request.TenantId, SearchKeyWord = request.ClientName };
                GetClientListResponse _client = await _clientFetchingGraphQLService.GetClientsByNameAsync(_getClientListModel);
                if (_client.TotalCount > 0)
                {
                    return new CreateClientResponse() { Success = false, Message = "The client already exists." };
                }

                if (string.IsNullOrEmpty(request.SecurityKey) || string.IsNullOrWhiteSpace(request.SecurityKey))
                {
                    if (request.SecurityAlgorithm == (int)SecurityAlgorithm.DES)
                    {
                        _securityKey = _securityKeyService.GenerateSecurityKey((int)SecurityAlgorithmLength.DES);
                    }
                    else if (request.SecurityAlgorithm == (int)SecurityAlgorithm.AES)
                    {
                        _securityKey = _securityKeyService.GenerateSecurityKey((int)SecurityAlgorithmLength.AES);
                    }
                    else
                    {
                        return new CreateClientResponse() { Success = false, Message = "There is no such security algorithm." };
                    }

                    request.SecurityKey = _securityKey;
                }
                else
                {
                    if (request.SecurityAlgorithm == (int)SecurityAlgorithm.DES && request.SecurityKey.Length == ((int)SecurityAlgorithmLength.DES + 1))
                    {

                    }
                    else if (request.SecurityAlgorithm == (int)SecurityAlgorithm.AES && request.SecurityKey.Length == ((int)SecurityAlgorithmLength.AES + 1))
                    {

                    }
                    else
                    {
                        return new CreateClientResponse() { Success = false, Message = "Invalid security key" };
                    }
                }

                _identityCode = await _clientDBService.GenerateClientIdentityAsync(request.TenantId, _dbConnection);
                if (string.IsNullOrWhiteSpace(_identityCode))
                {
                    return new CreateClientResponse() { Success = false, Message = "Failed to generate client identity code" };
                }

                if (!request.CountryId.HasValue)
                {
                    return new CreateClientResponse() { Success = false, Message = "Invalid country id." };
                }
                List<ProvinceCityPairModel> _provinceCityPairsList = await _getDictionaryListGraphQLService.GetProvinceCityPairListAsync(request.TenantId, request.CountryId??0);

                if (_provinceCityPairsList == null || _provinceCityPairsList.Count() == 0)
                {
                    return new CreateClientResponse() { Success = false, Message = "Invalid country id." };
                }
                else if (_provinceCityPairsList != null && request.StateOrProvinceId.HasValue)
                {
                    ProvinceCityPairModel p = _provinceCityPairsList.FirstOrDefault(_ => _.province == request.StateOrProvinceId);
                    if (p == null)
                    {
                        return new CreateClientResponse() { Success = false, Message = "Invalid province id." };
                    }
                    else if (request.CityId.HasValue)
                    {
                        ProvinceCityPairModel c = _provinceCityPairsList.FirstOrDefault(_ => _.province == request.StateOrProvinceId && _.city == request.CityId);
                        if (c == null)
                        {
                            return new CreateClientResponse() { Success = false, Message = "Invalid city id." };
                        }
                    }
                    else
                    {

                    }
                }

                Domain.Entities.Client client = _objectConvertingService.ConvertCreateClientRequestToClientEntity(request, _identityCode);
                Address address = _objectConvertingService.ConvertCreateClientRequestToAddressEntity(request);

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

                var message = _objectConvertingService.ConvertCreateClientRequestToClientMessageV1(request, ClientId, _identityCode);

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

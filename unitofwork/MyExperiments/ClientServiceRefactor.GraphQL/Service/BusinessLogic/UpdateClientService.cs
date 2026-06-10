using Domain.Enums;
using Domain.Models;
using FluentValidation;
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
        private readonly IGetDictionaryListGraphQLService _getDictionaryListGraphQLService;
        private readonly IClientFetchingGraphQLService _clientFetchingGraphQLService;
        private ClientModel _originalClient = null;
        private readonly IValidator<UpdateClientRequest> _validator;

        public UpdateClientService(IClientDBService clientDBService,
                                   ICoreService coreService,
                                   ICommonClientService commonClientService,
                                   IObjectConvertingService objectConvertingService,
                                   IGetDictionaryListGraphQLService getDictionaryListGraphQLService,
                                   IClientFetchingGraphQLService clientFetchingGraphQLService,
                                   IValidator<UpdateClientRequest> validator)
        {
            _clientDBService = clientDBService;
            _coreService = coreService;
            _commonClientService = commonClientService;
            _objectConvertingService = objectConvertingService;
            _getDictionaryListGraphQLService = getDictionaryListGraphQLService;
            _clientFetchingGraphQLService = clientFetchingGraphQLService;
            _validator = validator;
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

                var vresult = await _validator.ValidateAsync(request);

                if (!vresult.IsValid)
                {
                    string _errorMessage = vresult.Errors.FirstOrDefault().ErrorMessage;
                    return new UpdateClientResponse() { Success = false, Message = _errorMessage };
                }

                // initialize db connection
                var conn = await _coreService.GetDBConnection(request.TenantId);

                if (!conn.Success)
                    return new UpdateClientResponse() { Success = false, Message = "Error in Tenant DB" };

                _dbConnection = conn.Data;

                //check tx2 connector config
                var queueNameConfig = await _coreService.GetConfig("TX2ConnectorQueueName", request.TenantId);
                GetClientListModel _getClientListModelCheckingCode = new GetClientListModel { TenantId = request.TenantId, SearchKeyWord = request.IdentityCode };
                _originalClient = await _clientFetchingGraphQLService.GetClientsByCodeAsync(_getClientListModelCheckingCode);
                if (_originalClient == null)
                {
                    return new UpdateClientResponse() { Success = false, Message = "The client does not exist." };
                }

                if (string.IsNullOrEmpty(request.SecurityKey) || string.IsNullOrWhiteSpace(request.SecurityKey))
                {
                    return new UpdateClientResponse() { Success = false, Message = "Cannot make security key empty." };
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
                        return new UpdateClientResponse() { Success = false, Message = "Invalid security key" };
                    }
                }

                if (!request.CountryId.HasValue)
                {
                    return new UpdateClientResponse() { Success = false, Message = "Invalid country id." };
                }
                List<ProvinceCityPairModel> _provinceCityPairsList = await _getDictionaryListGraphQLService.GetProvinceCityPairListAsync(request.TenantId, request.CountryId ?? 0);

                if (_provinceCityPairsList == null || _provinceCityPairsList.Count() == 0)
                {
                    return new UpdateClientResponse() { Success = false, Message = "Invalid country id." };
                }
                else if (_provinceCityPairsList != null && request.StateOrProvinceId.HasValue)
                {
                    ProvinceCityPairModel p = _provinceCityPairsList.FirstOrDefault(_ => _.province == request.StateOrProvinceId);
                    if (p == null)
                    {
                        return new UpdateClientResponse() { Success = false, Message = "Invalid province id." };
                    }
                    else if (request.CityId.HasValue)
                    {
                        ProvinceCityPairModel c = _provinceCityPairsList.FirstOrDefault(_ => _.province == request.StateOrProvinceId && _.city == request.CityId);
                        if (c == null)
                        {
                            return new UpdateClientResponse() { Success = false, Message = "Invalid city id." };
                        }
                    }
                    else
                    {

                    }
                }

                client = _objectConvertingService.ConvertUpdateClientRequestToClientEntity(request);                

                if (request.CountryId.HasValue)
                {
                    address = _objectConvertingService.ConvertUpdateClientRequestToAddressEntity(request);
                }

                bool _updateSuccess = await _clientDBService.UpdateClientAsync(_originalClient, client, address, _dbConnection);
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

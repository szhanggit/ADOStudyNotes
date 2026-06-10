using Domain.Models;
using Repository;
using System.Data;
using TXC.Proto.Client;
using TXC.Proto.Credit;

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
        private readonly IClientRepository _clientRepository;
        private readonly ICoreService _coreService;
        private readonly ICommonClientService _commonClientService;
        private readonly IObjectConvertingService _objectConvertingService;

        public CreateClientService(
            IClientRepository clientRepository,
            ICoreService coreService,
            ICommonClientService commonClientService,
            IObjectConvertingService objectConvertingService)
        {
            _clientRepository = clientRepository;
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
                    PaddingCharacter = '0'
                };
                string identityCode = await _clientRepository.GenerateClientIdentityAsync(generateClientIdentityCode, _dbConnection);

                if (string.IsNullOrWhiteSpace(identityCode))
                {
                    return new CreateClientResponse() { Success = false, Message = "Failed to generate client identity code" };
                }

                identityCode = string.Concat(request.TenantId, identityCode); // Bug 4449


                Tuple<bool, string> result = await _clientRepository.CheckIfValidAddress(request.CityId, request.StateOrProvinceId, request.CountryId, _dbConnection);

                if (result.Item1 == false && request.CountryId.HasValue)
                {
                    return new CreateClientResponse() { Success = false, Message = result.Item2 };
                }

                int? ClientId = await _clientRepository.InsertClientAsync(request, identityCode, _dbConnection);

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
                    await _clientRepository.DeleteClientByIdAsync(ClientId.Value, _dbConnection);
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

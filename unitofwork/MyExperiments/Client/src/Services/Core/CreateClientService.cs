using AutoMapper;
using Dapper;
using Domain.Dto;
using Domain.Models;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Repository;
using Services.Constants;
using Services.Utility;
using Services.Validators;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TXC.Common.CacheManagement;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.MessageContract;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;
using TXC.Proto.Credit;

namespace Services.Core
{
    public delegate Task<Tuple<bool, string>> CheckAddressByCityDel(int? CityId, int? StateOrProvinceId, int? CountryId, IDbConnection _dbConnection);
    public interface ICreateClientService
    {
        public Task<ProtoBaseResponse> CreateClient(CreateClientRequest request);
    }

    public class CreateClientService : ICreateClientService
    {
        private readonly IValidator<CreateClientRequest> _validator;
        private string _securityKey = string.Empty;
        private IDbConnection _dbConnection;
        private readonly IClientRepository _clientRepository;
        private readonly ICoreService _coreService;
        private readonly ICommonClientService _commonClientService;
        private readonly IObjectConvertingService _objectConvertingService;
        private readonly ISecurityKeyService _securityKeyService;

        public CreateClientService(
            IClientRepository clientRepository,
            ICoreService coreService,
            ICommonClientService commonClientService,
            IObjectConvertingService objectConvertingService,
            ISecurityKeyService securityKeyService,
            IValidator<CreateClientRequest> validator)            
        {
            _clientRepository = clientRepository;
            _coreService = coreService;
            _commonClientService = commonClientService;
            _objectConvertingService = objectConvertingService;
            _securityKeyService = securityKeyService;
            _validator = validator;
        }

        public async Task<ProtoBaseResponse> CreateClient(CreateClientRequest request)
        {
            try
            {
                if (request.TenantId <= 0)
                    return new ProtoBaseResponse() { Success = false, Message = "TenantBasicInfoId header required" };

                if (string.IsNullOrEmpty(request.TenantName))
                    return new ProtoBaseResponse() { Success = false, Message = "TenantName header required" };

                var vresult = await _validator.ValidateAsync(request);

                if (!vresult.IsValid)
                {
                    return new ProtoBaseResponse() { Success = false, Message = vresult.Errors.FirstOrDefault().ErrorMessage};
                }

                // initialize db connection
                var conn = await _coreService.GetDBConnection(request.TenantId);

                if (!conn.Success)
                    return new ProtoBaseResponse() { Success = false, Message = "Error in Tenant DB" };

                _dbConnection = conn.Data;

                //check tx2 connector config
                var queueNameConfig = await _coreService.GetConfig("TX2ConnectorQueueName", request.TenantId);

                var checkAddress = await _clientRepository.CheckAddressAsync(request, _dbConnection);
                if (!string.IsNullOrEmpty(checkAddress))
                {
                    return new ProtoBaseResponse() { Success = false, Message = checkAddress };
                }

                GetClientListRequest getClientListRequest = new GetClientListRequest { SearchKeyword = request.ClientName };
                bool _hasTheSameClientName = await _clientRepository.CheckClientByNameAsync(getClientListRequest, _dbConnection);
                if (_hasTheSameClientName)
                {
                    return new ProtoBaseResponse() { Success = false, Message = "The client has been created." };
                }

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
                    return new ProtoBaseResponse() { Success = false, Message = "Failed to generate client identity code" };
                }

                identityCode = string.Concat(request.TenantId, identityCode); // Bug 4449


                Tuple<bool, string> result = await _clientRepository.CheckIfValidAddress(request.CityId, request.StateOrProvinceId, request.CountryId, _dbConnection);

                if (result.Item1 == false && request.CountryId.HasValue)
                {
                    return new ProtoBaseResponse() { Success = false, Message = result.Item2 };
                }

                if (string.IsNullOrEmpty(request.SecurityKey) || string.IsNullOrWhiteSpace(request.SecurityKey))
                {
                    _securityKey = _securityKeyService.GenerateSecurityKey((int)SecurityAlgorithmLength.DES);
                    request.SecurityKey = _securityKey;
                }

                int? ClientId = await _clientRepository.InsertClientAsync(request, identityCode, _dbConnection);

                if (!ClientId.HasValue)
                {
                    return new ProtoBaseResponse() { Success = false, Message = "Failed to create new client" };
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
                    return new ProtoBaseResponse() { Success = false, Message = "Failed to create new wallet" };
                }

                var message = _objectConvertingService.ConvertCreateClientRequestToClientMessageV1(request, ClientId, identityCode);

                //send to service bus
                bool _sendingResult = await _commonClientService.SendCreateMessageAsync(request.TenantId, queueNameConfig.Value, message);

                CreateClientResponse _createClientResponse = new CreateClientResponse { 
                    ClientId = ClientId ?? 0,
                    ClientCode = identityCode,
                    SecurityKey = request.SecurityKey
                };

                if (_sendingResult)
                {
                    return new ProtoBaseResponse { Success = true, Message = "Success", Data = Any.Pack(_createClientResponse) };
                }
                else
                {
                    return new ProtoBaseResponse() { Success = false, Message = "Fail to be sent to service bus", Data = Any.Pack(_createClientResponse) };
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using AutoMapper;
using Dapper;
using Domain.Models;
using Repository;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TXC.Common.CacheManagement;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.MessageContract;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;
using static Domain.Enums.Enums;

namespace Services.Core
{
    /// <summary>
    /// Interface CreateBXPClientService
    /// </summary>
    public interface ICreateBXPClientService
    {
        public Task<CreateBXPClientResponse> CreateBXPClient(CreateBXPClientRequest request);
    }

    /// <summary>
    /// Create BXPClientService
    /// </summary>
    public class CreateBXPClientService : ICreateBXPClientService
    {
        private readonly ITenantDbConnection _tenantDbConnection;
        private readonly IDapperOperation dapperOperation;
        private IDbConnection _dbConnection;
        private readonly ITX2ServiceBusSender _txcServiceBusSender;
        private readonly IMapper _mapper;
        private readonly ITenantConfigHelper _tenantConfigHelper;
        private ICommonClientService _commonClientService;
        private IClientRepository _clientRepository;
        private readonly CheckAddressByCityDel _checkAddress;
        private readonly IObjectConvertingService _objectConvertingService;
        private readonly ICoreService _coreService;

        public CreateBXPClientService(
            ITenantDbConnection tenantDbConnection, 
            IDapperOperation _dapperOperation, 
            ICommonClientService commonClientService, 
            IClientRepository clientRepository,
            ITX2ServiceBusSender txcServiceBusSender,
            ITenantConfigHelper tenantConfigHelper,
            IMapper mapper,
            IObjectConvertingService objectConvertingService,
            ICoreService coreService)
        {
            dapperOperation = _dapperOperation;
            _tenantDbConnection = tenantDbConnection;
            _txcServiceBusSender = txcServiceBusSender;
            _tenantConfigHelper = tenantConfigHelper;
            _mapper = mapper;
            _commonClientService = commonClientService;
            _clientRepository = clientRepository;
            _checkAddress = _clientRepository.CheckIfValidAddress;
            _objectConvertingService = objectConvertingService;
            _coreService = coreService;
        }

        /// <summary>
        /// CreateBXPClient
        /// </summary>
        /// <param name="request"></param>
        /// <param name="_checkAddress"></param>
        /// <returns></returns>
        public async Task<CreateBXPClientResponse> CreateBXPClient(CreateBXPClientRequest request)
        {
            try
            {
                if (request.TenantId <= 0)
                    return new CreateBXPClientResponse() { Success = false, Message = "TenantBasicInfoId header required" };

                if (string.IsNullOrEmpty(request.TenantName))
                    return new CreateBXPClientResponse() { Success = false, Message = "TenantName header required" };

                // initialize db connection
                var conn = await _coreService.GetDBConnection(request.TenantId);

                if (!conn.Success)
                    return new CreateBXPClientResponse() { Success = false, Message = "Error in Tenant DB" };

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
                    return new CreateBXPClientResponse() { Success = false, Message = "Failed to generate client identity code" };
                }

                identityCode = string.Concat(request.TenantId, identityCode); // Bug 4449


                Tuple<bool, string> result = await _checkAddress(request.CityId, request.StateOrProvinceId, request.CountryId, _dbConnection);

                if (result.Item1 == false)
                {
                    return new CreateBXPClientResponse() { Success = false, Message = result.Item2 };
                }

                string securityKey = SecurityKeyService.GenerateSecurityKey((int)SecurityAlgorithmLength.DES);
                Tuple<CreateBXPClientResponse, int?> _createBXPClientResult = await _clientRepository.CreateBXPClientAsync(request, securityKey, identityCode, _dbConnection);
                if (!_createBXPClientResult.Item1.Success)
                {
                    return _createBXPClientResult.Item1;
                }
             
                ClientMessageV1 message = _objectConvertingService.CreateBXPClientRequestToClientMessageV1(request, _createBXPClientResult.Item2, identityCode, securityKey);

                bool _sendingResult = await _commonClientService.SendCreateMessageAsync(request.TenantId, queueNameConfig.Value, message);
                if (_sendingResult)
                {
                    return new CreateBXPClientResponse { Success = true, Message = "Success", Data = _createBXPClientResult.Item2 ?? 0 };
                }
                else
                {
                    return new CreateBXPClientResponse() { Success = false, Message = "Fail to be sent to service bus", Data = _createBXPClientResult.Item2 ?? 0 };
                }                
            }
            catch (Exception exception)
            {
                return new CreateBXPClientResponse() { Success = false, Message = exception.Message };
            }
        }        
    }
}

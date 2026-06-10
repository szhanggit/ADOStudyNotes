using AutoMapper;
using Dapper;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.CacheManagement;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.MessageContract;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;

namespace Services.Core
{
    public interface IUpdateClientService
    {
        public Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request);
    }

    public class UpdateClientService : IUpdateClientService
    {
        private IDbConnection _dbConnection;
        private readonly IClientRepository _clientRepository;
        private readonly ICoreService _coreService;
        private readonly ICommonClientService _commonClientService;
        private readonly IObjectConvertingService _objectConvertingService;

        public UpdateClientService(IClientRepository clientRepository,
                                   ICoreService coreService,
                                   ICommonClientService commonClientService,
                                   IObjectConvertingService objectConvertingService)
        {
            _clientRepository = clientRepository;
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

                int RowCount = await _clientRepository.CheckClientIdAsync(request.ClientId, _dbConnection);
                if (RowCount != 1)
                {
                    return new UpdateClientResponse() { Success = false, Message = "The client does not exist." };
                }

                Tuple<bool, string> result = await _clientRepository.CheckIfValidAddress(request.CityId, request.StateOrProvinceId, request.CountryId, _dbConnection);

                if (result.Item1 == false && request.CountryId.HasValue)
                {
                    return new UpdateClientResponse() { Success = false, Message = result.Item2 };
                }

                int dbaffectedRows = await _clientRepository.UpdateClientAsync(request, _dbConnection);
                if (dbaffectedRows < 1)
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

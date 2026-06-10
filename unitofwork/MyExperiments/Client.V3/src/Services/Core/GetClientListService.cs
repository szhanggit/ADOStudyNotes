using Google.Protobuf.WellKnownTypes;
using Repository;
using System;
using System.Data;
using System.Threading.Tasks;
using TXC.Proto.Client;

namespace Services.Core
{
    public interface IGetClientListService
    {
        public Task<ProtoBaseResponse> GetClientList(GetClientListRequest request);
    }

    public class GetClientListService : IGetClientListService
    {
        private IDbConnection _dbConnection;
        private readonly IClientRepository _clientRepository;
        private readonly ICoreService _coreService;

        public GetClientListService(
            IClientRepository clientRepository,
            ICoreService coreService)
        {
            _clientRepository = clientRepository;
            _coreService = coreService;
        }


        public async Task<ProtoBaseResponse> GetClientList(GetClientListRequest request)
        {
            try
            {
                if (request.TenantId <= 0)
                    return new ProtoBaseResponse() { Success = false, Message = "TenantBasicInfoId header required" };

                if (string.IsNullOrEmpty(request.TenantName))
                    return new ProtoBaseResponse() { Success = false, Message = "TenantName header required" };

                // checkers for default values in pagination
                if (request.PageNumber == 0 || request.PageNumber == null)
                    request.PageNumber = 1;
                if (request.RowCount == 0 || request.RowCount == null)
                    request.RowCount = 20;

                // initialize db connection
                var conn = await _coreService.GetDBConnection(request.TenantId);

                if (!conn.Success)
                    return new ProtoBaseResponse() { Success = false, Message = "Error in Tenant DB" };

                _dbConnection = conn.Data;
                var dbResult = await _clientRepository.GetClientAsync(request, _dbConnection);

                GetClientListResponse response = new GetClientListResponse();
                response.ClientDtos.AddRange(dbResult.Item2);
                response.TotalCount = dbResult.Item1;

                return new ProtoBaseResponse
                {
                    Success = true,
                    Message = "Success",
                    Data = Any.Pack(response)
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

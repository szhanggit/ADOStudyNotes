using Domain.Dtos;
using Grpc.Core;

/*Dapper Start*/
using Repository.Dapper;
using Service.BusinessLogic;
/*Dapper End*/

/*EF Start*/
//using Repository.EF;
/*EF End*/

using System.Data;
using System.Data.SqlClient;
using TXC.Common.Data;
using TXC.Proto.Client;

namespace Service.gRPCController
{
    public class ClientService : Client.ClientBase
    {
        private readonly ICreateClientService _createClientService;
        private readonly IUpdateClientService _updateClientService;
        private readonly IGetClientListService _getClientListService;
        private readonly ICreateBXPClientService _createBXPClientService;

        public ClientService(
                ICreateClientService createClientService,
                IUpdateClientService updateClientService,
                IGetClientListService getClientListService,
                ICreateBXPClientService createBXPClientService,
                IConfiguration configuration
            )
        {
            _createClientService = createClientService;
            _updateClientService = updateClientService;
            _getClientListService = getClientListService;
            _createBXPClientService = createBXPClientService;
        }

        public override async Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
        {
            return await _createClientService.CreateClient(request);
        }

        public override async Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request, ServerCallContext context)
        {
            return await _updateClientService.UpdateClient(request);
        }

        public override async Task<ProtoBaseResponse> GetClientList(GetClientListRequest request, ServerCallContext context)
        {
            return await _getClientListService.GetClientList(request);
        }

        public override async Task<CreateBXPClientResponse> CreateBXPClient(CreateBXPClientRequest request, ServerCallContext context)
        {
            return await _createBXPClientService.CreateBXPClient(request);
        }

    }
}

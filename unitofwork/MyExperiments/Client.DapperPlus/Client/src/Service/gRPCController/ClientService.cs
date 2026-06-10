using Grpc.Core;
using Service.BusinessLogic;
using TXC.Proto.Client;

namespace Service.gRPCController
{
    public class ClientService : Client.ClientBase
    {
        private readonly ICreateClientService _createClientService;
        private readonly IUpdateClientService _updateClientService;
        private readonly IGetClientListService _getClientListService;

        public ClientService(
                ICreateClientService createClientService,
                IUpdateClientService updateClientService,
                IGetClientListService getClientListService,
                IConfiguration configuration
            )
        {
            _createClientService = createClientService;
            _updateClientService = updateClientService;
            _getClientListService = getClientListService;
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

    }
}

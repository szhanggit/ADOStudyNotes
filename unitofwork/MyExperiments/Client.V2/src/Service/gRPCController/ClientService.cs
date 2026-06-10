//using Services.Core;
//using System.Threading.Tasks;
using Grpc.Core;
using TXC.Common.Data;
using TXC.Proto.Client;

namespace Service.gRPCController
{
    public class ClientService : Client.ClientBase
    {
        //private readonly ICreateClientService _createClientService;
        //private readonly IUpdateClientService _updateClientService;
        //private readonly IGetClientListService _getClientListService;
        //private readonly ICreateBXPClientService _createBXPClientService;
        //private readonly ICommonClientService _commonClientService;
        //private IClientRepository _clientRepository;
        private readonly IDapperOperation _dapperOperation;



        public ClientService(
            //ICreateClientService createClientService,
            //                 IUpdateClientService updateClientService,
            //                 IGetClientListService getClientListService,
            //                 IClientRepository clientRepository,
            //                 ICreateBXPClientService createBXPClientService,
            //                 IDapperOperation dapperOperation,
            //                 ICommonClientService commonClientService
            )
        {
            //_createClientService = createClientService;
            //_updateClientService = updateClientService;
            //_getClientListService = getClientListService;
            //_commonClientService = commonClientService;
            //_createBXPClientService = createBXPClientService;
        }

        public override async Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
        {
            //return await _createClientService.CreateClient(request);
            return await base.CreateClient(request, context);
        }

        public override async Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request, ServerCallContext context)
        {
            //return await _updateClientService.UpdateClient(request);
            return await base.UpdateClient(request, context);
        }

        public override async Task<ProtoBaseResponse> GetClientList(GetClientListRequest request, ServerCallContext context)
        {
            //return await _getClientListService.GetClientList(request);
            return await base.GetClientList(request, context);
        }

        public override async Task<CreateBXPClientResponse> CreateBXPClient(CreateBXPClientRequest request, ServerCallContext context)
        {
            //return await _createBXPClientService.CreateBXPClient(request);
            return await base.CreateBXPClient(request, context);
        }

    }
}

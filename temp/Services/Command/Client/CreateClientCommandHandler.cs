using AutoMapper;
using Microsoft.AspNetCore.Http;
using Repository;
using Services.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;
using TXC.Proto.Client;

namespace Services.Command.Client
{
    public class CreateClientCommandHandler : IRequestHandlerWrapper<CreateClientCommand, int>
    {
        
        private readonly IMapper _mapper;
        private readonly ICreateClientService _createClientService;
        private readonly string _TX2UserName;
        private readonly string _TenantName;
        private readonly int _TenantId;
        private CommonClientService _commonClientService;
        private IClientRepository _clientRepository;
        private readonly IDapperOperation _dapperOperation;


        public CreateClientCommandHandler(IMapper mapper,
                                          ICreateClientService createClientService,
                                          IDapperOperation dapperOperation,
                                          IHttpContextAccessor httpContextAccessor,
                                          IClientRepository clientRepository)
        {
            _mapper = mapper;
            _createClientService = createClientService;
            _TX2UserName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TX2UserName];
            _TenantName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantName];
            Int32.TryParse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId], out _TenantId);
            _clientRepository = clientRepository;
        }

        public async Task<Response<int>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var grpcRequest = _mapper.Map<CreateClientRequest>(request);
                grpcRequest.TX2UserName = _TX2UserName;
                grpcRequest.TenantName = _TenantName;
                grpcRequest.TenantId = _TenantId;
                var grpcResponse = await _createClientService.CreateClient(grpcRequest);

                return Response.Success(grpcResponse.Message, grpcResponse.Data);
            }
            catch (Exception exception)
            {
                return Response.Fail(exception.Message, 0);
            }
        }
    }
}
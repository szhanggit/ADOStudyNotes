using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Repository;
using Services.Core;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Domain;
using TXC.Common.MessageContract;
using TXC.Common.MessageContract.Client;
using TXC.Common.Services.Wrappers;
using TXC.Proto.Client;

namespace Services.Command.Client
{
    public class UpdateClientCommandHandler : IRequestHandlerWrapper<UpdateClientCommand, int>
    {
        
        private readonly IMapper _mapper;
        private readonly IUpdateClientService _updateClientService;
        private readonly string _TX2UserName;
        private readonly string _TenantName;
        private readonly int _TenantId;
        private readonly IDapperOperation _dapperOperation;

        private CommonClientService _commonClientService;
        private IClientRepository _clientRepository;

        public UpdateClientCommandHandler(IMapper mapper,
                                          IUpdateClientService updateClientService, 
                                          IDapperOperation dapperOperation,
                                          IHttpContextAccessor httpContextAccessor,
                                          IClientRepository clientRepository)
        {
            _mapper = mapper;
            _updateClientService = updateClientService;
            _TX2UserName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TX2UserName];
            _TenantName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantName];
            Int32.TryParse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId], out _TenantId);
            _clientRepository = clientRepository;
            _dapperOperation = dapperOperation;
        }

        public async Task<Response<int>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var grpcRequest = _mapper.Map<UpdateClientRequest>(request);
                grpcRequest.TX2UserName = _TX2UserName;
                grpcRequest.TenantName = _TenantName;
                grpcRequest.TenantId = _TenantId;
                var grpcResponse = await _updateClientService.UpdateClient(grpcRequest);

                return Response.Success(grpcResponse.Message, grpcResponse.Data);
            }
            catch (Exception exception)
            {
                return Response.Fail(exception.Message, 0);
            }
        }
    }
}
using AutoMapper;
using Dapper;
using Domain.Models.Response;
using Services.Core;
using Services.Queries.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Domain;
using TXC.Common.Services;
using TXC.Common.Services.Wrappers;
using TXC.Proto.Client;

namespace Services.Command.Client
{
    public class GetClientListQueryHandler : IRequestListHandlerWrapper<GetClientListQuery, Domain.Models.Response.GetClientListResponse>
    {
        private readonly IMapper _mapper;
        private readonly IGetClientListService _getClientListService;

        public GetClientListQueryHandler(IMapper mapper,
                                         IGetClientListService getClientListService)
        {
            _mapper = mapper;
            _getClientListService = getClientListService;
        }

        public async Task<Response<Domain.Models.Response.GetClientListResponse>> Handle(GetClientListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var grpcRequest = _mapper.Map<GetClientListRequest>(request);
                var grpcResponse = await _getClientListService.GetClientList(grpcRequest);

                return new Response<Domain.Models.Response.GetClientListResponse>()
                {
                    Success = grpcResponse.Success,
                    Message = grpcResponse.Message,
                    Data = _mapper.Map<Domain.Models.Response.GetClientListResponse>(grpcResponse.Data.Unpack<TXC.Proto.Client.GetClientListResponse>())
                };
            }
            catch (Exception ex)
            {
                return new Response<Domain.Models.Response.GetClientListResponse>() { Success = false, Message = ex.Message, Data = null };
            }
        }
    }
}

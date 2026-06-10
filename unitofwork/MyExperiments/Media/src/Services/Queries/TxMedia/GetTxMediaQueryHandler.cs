using AutoMapper;
using Domain.Dto;
using Microsoft.AspNetCore.Http;
using Services.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;
using TXC.Proto.Media;

namespace Services.Queries.TxMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetTxMediaQueryHandler : IRequestHandlerWrapper<GetTxMediaQuery, TxMediaDto>
    {
        private readonly IMapper _mapper;
        private readonly GetMediaByIdService _getMediaByIdService;
        private readonly int _tenantId;
        public GetTxMediaQueryHandler(IMapper mapper,
            GetMediaByIdService getMediaByIdService,
            IHttpContextAccessor httpContextAccessor) 
        {
            _mapper = mapper;
            _getMediaByIdService = getMediaByIdService;
            _tenantId = int.Parse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId]);
        }

        public async Task<Response<TxMediaDto>> Handle(GetTxMediaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var grpcRequest = _mapper.Map<GetMediaByIdRequest>(request);
                grpcRequest.TenantId = _tenantId;
                var grpcResponse = await _getMediaByIdService.GetMediaId(grpcRequest);

                if (grpcResponse.Success)
                {
                    var data = grpcResponse.Data.Unpack<GetMediaByIdResponse>();
                    return Response.Success("success", _mapper.Map<TxMediaDto>(data));
                }

                return Response.Success<TxMediaDto>("success", null);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

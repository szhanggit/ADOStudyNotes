using AutoMapper;
using Domain.Dto;
using Microsoft.AspNetCore.Http;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;
using TXC.Proto.Media;

namespace Services.Queries.TxMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetTxMediaListQueryHandler : IRequestHandlerWrapper<GetTxMediaListQuery, IEnumerable<TxMediaDto>>
    {
        private readonly IMapper _mapper;
        private readonly GetAllMediaService _getAllMediaService;
        private readonly int _tenantId;
        public GetTxMediaListQueryHandler(IMapper mapper,
            GetAllMediaService getAllMediaService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _getAllMediaService = getAllMediaService;
            _tenantId = int.Parse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId]);
        }

        public async Task<Response<IEnumerable<TxMediaDto>>> Handle(GetTxMediaListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var grpcRequest = _mapper.Map<GetAllMediaRequest>(request);
                grpcRequest.TenantId = _tenantId;
                var grpcResponse = await _getAllMediaService.GetAllMedia(grpcRequest);

                if (grpcResponse.Success)
                {
                    var data = grpcResponse.Data == null ? null : grpcResponse.Data.Unpack<GetAllMediaResponse>();
                    return Response.Success("success", _mapper.Map<IEnumerable<TxMediaDto>>(data.MediaItems));
                }


                return Response.Success<IEnumerable<TxMediaDto>>("success", null);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

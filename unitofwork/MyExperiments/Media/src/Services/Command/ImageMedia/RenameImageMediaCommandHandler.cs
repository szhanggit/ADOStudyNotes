using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.CacheManagement.Interface;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;
using TXC.Proto.Media;

namespace Services.Command.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class RenameImageMediaCommandHandler : IRequestHandlerWrapper<RenameImageMediaCommand, int>
    {

        private readonly string _tenantName;
        private readonly string _idempotentKey;
        private readonly int _tenantId;
        private readonly string _TX2UserName;
        private readonly ICacheOperation _cacheOperation;
        private readonly ILogger<RenameImageMediaCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly RenameMediaService _renameMediaService; 

        public RenameImageMediaCommandHandler(IHttpContextAccessor httpContextAccessor,
            ICacheOperation cacheOperation,
            ILogger<RenameImageMediaCommandHandler> logger,
            IMapper mapper,
            RenameMediaService renameMediaService)
        {
            _tenantName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantName];
            _idempotentKey = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.Idempotent];
            _tenantId = int.Parse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId]);
            _TX2UserName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TX2UserName];
            _cacheOperation = cacheOperation;
            _logger = logger;
            _mapper = mapper;
            _renameMediaService = renameMediaService;   
        }

        public async Task<Response<int>> Handle(RenameImageMediaCommand request, CancellationToken cancellationToken)
        {
            try
            {

                await _cacheOperation.SetCacheAsync(_idempotentKey, new Response<int>(0, "Request processing", true), cancellationToken);
                var grpcRequest = _mapper.Map<RenameMediaRequest>(request);
                grpcRequest.TenantId = _tenantId;

                var grpcResponse = await _renameMediaService.RenameMedia(grpcRequest);

                if (grpcResponse.Success)
                { 
                    var responseData = grpcResponse.Data.Unpack<RenameMediaResponse>();
                    return Response.Success("Success", responseData.MediaId);
                }

                return Response.Fail("fail", 0);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "RenameImageMediaCommandHandler Error");
                return Response.Fail<int>("Exception", 0);
            }
        }
    }
}
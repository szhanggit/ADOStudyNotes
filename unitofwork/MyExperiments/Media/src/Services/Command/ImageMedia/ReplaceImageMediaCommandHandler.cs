using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Command.Media.Extensions;
using Services.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;
using TXC.Proto.Media;

namespace Services.Command.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class ReplaceImageMediaCommandHandler : IRequestHandlerWrapper<ReplaceImageMediaCommand, int>
    {
        private readonly string _tenantName;
        private readonly int _tenantId;
        private readonly string _TX2UserName;
        private readonly ILogger<ReplaceImageMediaCommandHandler> _logger;
        private readonly ReplaceMediaService _replaceMediaService;

        public ReplaceImageMediaCommandHandler(IHttpContextAccessor httpContextAccessor,
            ILogger<ReplaceImageMediaCommandHandler> logger,
            ReplaceMediaService replaceMediaService) 
        {
            _tenantName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantName];
            _tenantId = int.Parse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId]);
            _TX2UserName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TX2UserName];
            _logger = logger;
            _replaceMediaService = replaceMediaService;
        }

        public async Task<Response<int>> Handle(ReplaceImageMediaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(_tenantName) || string.IsNullOrEmpty(_tenantId.ToString()))
                {
                    return Response.Fail("TenantName and TenantBasicInfoId header required", 0);
                }

                var grpcRequest = new ReplaceMediaRequest
                {
                    BlobName = request.BlobName,
                    FileName = request.Image.FileName,
                    ContentType = request.Image.ContentType,
                    Image = ByteString.FromStream(request.Image.OpenReadStream()),
                    ImageHeight = request.Image.GetImageHeight(),
                    ImageWidth = request.Image.GetImageWidth(),
                    MediaId = request.MediaId,
                    TenantId = _tenantId,
                    TenantName = _tenantName
                };

                var grpcResponse = await _replaceMediaService.ReplaceMedia(grpcRequest);
                if (grpcResponse.Success)
                {
                    var responseData = grpcResponse.Data == null ? null : grpcResponse.Data.Unpack<ReplaceMediaResponse>();
                    return Response.Success("success",responseData.MediaId);
                }

                return Response.Fail(grpcResponse.Message, 0);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ReplaceImageMediaCommandHandler Error");
                return Response.Fail<int>("Exception", 0);
            }
        }
    }
}
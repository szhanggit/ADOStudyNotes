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
    public class CreateImageMediaCommandHandler : IRequestHandlerWrapper<CreateImageMediaCommand, int>
    {
        private readonly string _tenantName;
        private readonly int _tenantId;
        private readonly string _TX2UserName;
        private readonly ILogger<CreateImageMediaCommandHandler> _logger;
        private readonly CreateMediaService _createMediaService;
        public CreateImageMediaCommandHandler(IHttpContextAccessor httpContextAccessor,
            ILogger<CreateImageMediaCommandHandler> logger,
            CreateMediaService createMediaService) 
        {

            _tenantId = int.Parse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId]);
            _tenantName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantName];
            _TX2UserName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TX2UserName];
            _logger = logger;
            _createMediaService = createMediaService;

        }

        public async Task<Response<int>> Handle(CreateImageMediaCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var grpcRequest = new CreateMediaRequest
                {
                    Type = (int)request.Type,
                    Image = ByteString.FromStream(request.Image.OpenReadStream()),
                    TenantId = _tenantId,
                    TenantName = _tenantName,
                    FileName = request.Image.FileName,
                    ContentType = request.Image.ContentType,
                    ImageHeight = request.Image.GetImageHeight(),
                    ImageWidth = request.Image.GetImageWidth(),
                    TX2UserName = _TX2UserName,
                };

                var grpcResponse = await _createMediaService.CreateMedia(grpcRequest);
                if (grpcResponse.Success)
                {
                    var responseData = grpcResponse.Data == null ? null : grpcResponse.Data.Unpack<CreateMediaResponse>();
                    return Response.Success("success", responseData.MediaId);

                }

                return Response.Fail<int>(grpcResponse.Message, 0);


            }
            catch (Exception e)
            {
                _logger.LogError(e, "CreateImageMediaCommandHandler Error");
                return Response.Fail<int>("Exception", 0);
            }
        }
    }
}
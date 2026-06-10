using Grpc.Core;
using Services.Core;
using System.Threading.Tasks;
using TXC.Proto.Media;

namespace Services.GrpcService
{
    public class MediaService : Media.MediaBase
    {
        private readonly CreateMediaService _createMediaService;
        private readonly ReplaceMediaService _replaceMediaService;
        private readonly RenameMediaService _renameMediaService;
        private readonly GetMediaByIdService _getMediaByIdService;
        private readonly GetAllMediaService _getAllMediaService;
        public MediaService(CreateMediaService createMediaService,
            ReplaceMediaService replaceMediaService,
            RenameMediaService renameMediaService,
            GetMediaByIdService getMediaByIdService,
            GetAllMediaService getAllMediaService)
        {
            _createMediaService = createMediaService;
            _replaceMediaService = replaceMediaService;
            _renameMediaService = renameMediaService;
            _getMediaByIdService = getMediaByIdService;
            _getAllMediaService = getAllMediaService;
        }

        public override async Task<ProtoBaseResponse> CreateMedia(CreateMediaRequest request, ServerCallContext context)
        {
            return await _createMediaService.CreateMedia(request);
        }

        public override async Task<ProtoBaseResponse> ReplaceMedia(ReplaceMediaRequest request, ServerCallContext context)
        {
            return await _replaceMediaService.ReplaceMedia(request);
        }

        public override async Task<ProtoBaseResponse> RenameMedia(RenameMediaRequest request, ServerCallContext context)
        {
            return await _renameMediaService.RenameMedia(request);
        }

        public override async Task<ProtoBaseResponse> GetMediaById(GetMediaByIdRequest request, ServerCallContext context)
        {
            return await _getMediaByIdService.GetMediaId(request);
        }

        public override async Task<ProtoBaseResponse> GetAllMedia(GetAllMediaRequest request, ServerCallContext context)
        {
            return await _getAllMediaService.GetAllMedia(request);
        }


    }
}

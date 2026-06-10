using Core;
using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace RepositoryEf.Repositories
{
    public class MediaRepo
    {
        public interface IMediaRepository : IRepository<Media>
        {
            /// <summary>
            /// This method used LINQ to get media by id
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            Task<Media> GetMediaById(int id);

            /// <summary>
            /// This method used Stored Procedure to get media by id
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            Task<MediaResponseDto> GetMediaById(MediaRequestDto request);
        }

        public class MediaRepository : Repository<Media>, IMediaRepository
        {
            public MediaRepository(MediaContext context) : base(context)
            {
            }

            public async Task<Media> GetMediaById(int id) => await _context.Set<Media>().FindAsync(id);

            public async Task<MediaResponseDto> GetMediaById(MediaRequestDto request)
            {
                var param = new SqlParameter[]
                {
                    new SqlParameter("@MediaId",request.Id)
                };
                //var result = await Task.FromResult(_context
                //    .Set<MediaResponseDto>()
                //    .FromSqlRaw($@"[media].[sp_sel_media] @MediaId", param));
                var result = await Task.FromResult(_context
                .Set<MediaResponseDto>()
                .FromSqlRaw($@"Select media_id,
		                [file_name],
		                keyword AS Keyword,
		                height AS Height,
		                width AS Width,
		                blob_name AS BlobName,
		                [type] AS [Type],
		                node_url
	                FROM media.tb_m_media WHERE media_id = @MediaId	", param));

                return result.AsEnumerable().FirstOrDefault();
            }

        }
    }
}

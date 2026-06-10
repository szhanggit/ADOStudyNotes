
using Domain.CustomTypes.Request;
using Domain.CustomTypes.Response;
using Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TXC.Common.RepositoryCore;

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
            Task<MediaResponse> GetMediaById(MediaRequest request);
        }

        public class MediaRepository : Repository<Media>, IMediaRepository
        {
            public MediaRepository(MediaContext context) : base(context)
            {
            }

            public async Task<Media> GetMediaById(int id) => await _context.Set<Media>().FindAsync(id);

            public async Task<MediaResponse> GetMediaById(MediaRequest request)
            {
                var param = new SqlParameter[]
                {
                    new SqlParameter("@MediaId",request.Id)
                };
                var result = await Task.FromResult(_context
                    .Set<MediaResponse>()
                    .FromSqlRaw("[media].[sp_sel_media] @MediaId", param));

                return result.AsEnumerable().FirstOrDefault();
            }

        }
    }
}

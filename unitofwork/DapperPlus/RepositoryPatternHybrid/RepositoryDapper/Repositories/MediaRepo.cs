using Dapper;
using Dapper.Contrib.Extensions;
using Domain.CustomTypes.Request;
using Domain.CustomTypes.Response;
using Domain.Models;
using TXC.Common.RepositoryCore;

namespace RepositoryDapper.Repositories
{
    public class MediaRepo
    {
        public interface IMediaRepository : IRepository<Media>
        {
            Task<Media> GetMediaById(int id);
            Task<MediaResponse> GetMediaById(MediaRequest request);
        }

        public class MediaRepository : Repository<Media>, IMediaRepository
        {
            public MediaRepository(MediaContext context) : base(context)
            {
            }
            public async Task<Media> GetMediaById(int id)
            {
                return await _context.Connection.GetAsync<Media>(id);
            }

            public async Task<MediaResponse> GetMediaById(MediaRequest request)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@MediaId", request.Id);

                //var cmd = new CommandDefinition(commandText: "[media].[sp_sel_media] @MediaId", parameters: param);
                var cmd = new CommandDefinition(commandText: "Select * from media.tb_m_media where media_id = @MediaId", parameters: param, commandType: System.Data.CommandType.Text);
                return await _context.Connection.QueryFirstOrDefaultAsync<MediaResponse>(cmd);
            }


        }
    }
}

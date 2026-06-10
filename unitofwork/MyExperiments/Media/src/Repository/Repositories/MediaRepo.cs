using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.RepositoryCore;

namespace Repository.Repositories
{
    public class MediaRepo
    {
        public interface IMediaRepository : IRepository<Media>
        {
        }

        public class MediaRepository : Repository<Media>, IMediaRepository
        {
            public MediaRepository(Context context) : base(context)
            {
            }
        }
    }
}

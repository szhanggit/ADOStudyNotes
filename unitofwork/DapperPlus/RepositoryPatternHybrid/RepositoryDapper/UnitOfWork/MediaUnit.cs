using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.RepositoryCore;
using static RepositoryDapper.Repositories.MediaRepo;

namespace RepositoryDapper.UnitOfWork
{
    public class MediaUnit
    {
        public interface IMediaUnitOfWork : IUnitOfWork
        {
            IMediaRepository MediaRepository { get; }
        }
        public class MediaUnitOfWork : IMediaUnitOfWork
        {
            private readonly MediaContext _context;
            public MediaUnitOfWork(MediaContext context,
                IMediaRepository mediaRepository)
            {
                _context = context;
                MediaRepository = mediaRepository;
            }
            public IMediaRepository MediaRepository { get; }
            public void SetConnection(string connectionString) => _context.SetConnection(connectionString);
            public async Task<int> Complete()
            {
                return await Task.FromResult(0);
            }

            public void Dispose()
            {
                _context.Dispose();
            }
        }
    }
}

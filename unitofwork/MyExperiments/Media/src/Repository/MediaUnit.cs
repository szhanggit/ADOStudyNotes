using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.RepositoryCore;
using static Repository.Repositories.MediaRepo;

namespace Repository
{
    public class MediaUnit
    {
        public interface IMediaUnitOfWork : IDisposable, IUnitOfWork
        {
            IMediaRepository MediaRepository { get; }
        }
        public class MediaUnitOfWork : IMediaUnitOfWork
        {
            private readonly Context _context;
            public MediaUnitOfWork(Context context,
                IMediaRepository mediaRepository)
            {
                _context = context;
                MediaRepository = mediaRepository;
            }
            public IMediaRepository MediaRepository { get; }
            /// <summary>
            /// Sets the connection by string
            /// </summary>
            /// <param name="connectionString"></param>
            public void SetConnection(string connectionString) => _context.SetConnection(connectionString);
            /// <summary>
            /// Sets the connection by new Connection instance like (new SqlConnection())
            /// </summary>
            /// <param name="connection"></param>
            public void SetConnection(IDbConnection connection) => _context.SetConnection(connection);
        
            public void Dispose()
            {
                _context.Dispose();
            }

            /// <summary>
            /// For future implementation if in case the developer needs to do transactional query.
            /// </summary>
            /// <returns></returns>
            public async Task<int> Complete()
            {
                if(_context.Transaction != null)
                    await Task.Factory.StartNew(()=> _context.Transaction.Complete());

                return 0;
            }
        }
    }
}

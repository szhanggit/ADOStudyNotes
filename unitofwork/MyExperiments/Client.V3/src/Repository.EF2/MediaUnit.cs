namespace Repository.EF2
{
    public interface IMediaUnitOfWork : IUnitOfWork
    {
        IMediaRepository MediaRepository { get; }
        MediaContext Context{ get; }
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
        public IMediaRepository MediaRepository { get; private set; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public MediaContext Context
        {
            get{ return _context; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SetConnection(string connectionString) => _context.SetConnection(connectionString);
    }
}
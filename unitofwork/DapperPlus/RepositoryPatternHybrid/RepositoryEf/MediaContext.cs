using Domain.CustomTypes.Response;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using TXC.Common.RepositoryCore;

namespace RepositoryEf
{
    public class MediaContext : DbContext, IContextProvider
    {
        public MediaContext(DbContextOptions<MediaContext> options) : base(options)
        {
        }
        public void SetConnection(string connectionString) => Database.SetConnectionString(connectionString);

        public DbSet<Media>? Medias { get; set; }
        public DbSet<MediaResponse>? MediaDtos { get; set; }
    }
}

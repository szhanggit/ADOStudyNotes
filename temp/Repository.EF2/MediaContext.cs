using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF2
{
    public class MediaContext : DbContext, IContextProvider
    {
        public MediaContext(DbContextOptions<MediaContext> options) : base(options)
        {
        }
        public void SetConnection(string connectionString) => Database.SetConnectionString(connectionString);

        public DbSet<Media>? Medias { get; set; }
        public DbSet<MediaResponseDto>? MediaDtos { get; set; }
        public DbSet<ClientEF>? ClientEF { get; set; }
    }
}

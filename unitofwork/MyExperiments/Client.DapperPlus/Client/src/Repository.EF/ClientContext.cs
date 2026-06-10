using Core;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF
{
    public class ClientContext : DbContext, IContextProvider
    {
        public ClientContext(DbContextOptions<ClientContext> options) : base(options)
        {
        }
        public void SetConnection(IDbConnection conn) => Database.SetConnectionString(conn.ConnectionString);

        public DbSet<Client>? Client { get; set; }
    }
}

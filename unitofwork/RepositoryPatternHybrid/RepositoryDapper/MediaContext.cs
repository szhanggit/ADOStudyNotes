using Core;
using Z.Dapper.Plus;

namespace RepositoryDapper
{
    public class MediaContext : DapperPlusContext, IContextProvider
    {
        public MediaContext()
        {
        }

        public void Dispose()
        {
            Connection.Dispose();
        }


        public void SetConnection(string connectionString) => Connection.ConnectionString = connectionString;
    }
}

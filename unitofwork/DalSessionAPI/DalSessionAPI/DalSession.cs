using System.Data;
using System.Data.SqlClient;

namespace DalSessionAPI
{
    public sealed class DalSession : IDisposable
    {
        public DalSession()
        {
            _connection = new SqlConnection("Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_gl;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true");
            _connection.Open();
            _unitOfWork = new UnitOfWork(_connection);
        }

        IDbConnection _connection = null;
        UnitOfWork _unitOfWork = null;

        public UnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}

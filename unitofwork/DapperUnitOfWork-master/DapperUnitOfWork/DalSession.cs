using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperUnitOfWork
{
    public sealed class DalSession : IDisposable
    {
        public DalSession()
        {
            _connection = new OleDbConnection("");
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

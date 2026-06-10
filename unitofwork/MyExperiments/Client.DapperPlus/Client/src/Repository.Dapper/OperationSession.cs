using Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Dapper
{
    public interface IOperationSession
    {
        IClientRepository clientRepository { get; }
        IAddressRepository addressRepository { get; }
        IDictionaryRepository dictionaryRepository { get; }
        IUnitOfWork UnitOfWork { get; }
        void SetConnection(IDbConnection conn);
        void Dispose();
    }
    public class OperationSession : IOperationSession, IDisposable, IContextProvider
    {
        private IDbConnection _connection = null;
        private IUnitOfWork _unitOfWork = null;
        private IClientRepository _clientRepository = null;
        private IAddressRepository _addressRepository = null;
        private IDictionaryRepository _dictionaryRepository = null;

        public OperationSession()
        {

        }

        public IClientRepository clientRepository { get { return _clientRepository; } }
        public IAddressRepository addressRepository { get { return _addressRepository; } }
        public IDictionaryRepository dictionaryRepository { get { return _dictionaryRepository; } }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public void SetConnection(IDbConnection conn)
        {
            _connection = conn;
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            _unitOfWork = new UnitOfWork();
            _unitOfWork.SetConnection(_connection);
            _unitOfWork.BeginAsync();
            _clientRepository = new ClientRepository(_unitOfWork);
            _addressRepository = new AddressRepository(_unitOfWork);
            _dictionaryRepository = new DictionaryRepository(_unitOfWork);
        }

        public void Dispose()
        {
            if(_unitOfWork != null)
                _unitOfWork.Dispose();
            
            if(_connection != null)
                _connection.Dispose();
        }
    }
}

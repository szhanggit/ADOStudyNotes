using Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OperationSession : IDisposable, IContextProvider
    {
        private IDbConnection _connection = null;
        private IUnitOfWork _unitOfWork = null;
        private IClientBasicInfoRepository _clientBasicInfoRepository = null;
        private IAddressRepository _addressRepository = null;
        private IDictionaryRepository _dictionaryRepository = null;

        public OperationSession(string _connectionString)
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            _unitOfWork = new UnitOfWork(_connection as DbConnection);
            _unitOfWork.BeginAsync();
            _clientBasicInfoRepository = new ClientBasicInfoRepository(_unitOfWork);
            _addressRepository = new AddressRepository(_unitOfWork);
            _dictionaryRepository = new DictionaryRepository(_unitOfWork);
        }

        public IClientBasicInfoRepository clientBasicInfoRepository { get {return _clientBasicInfoRepository;} }
        public IAddressRepository addressRepository { get {return _addressRepository;} }
        public IDictionaryRepository dictionaryRepository { get {return _dictionaryRepository;} }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public void SetConnection(IDbConnection conn)
        {
            
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}

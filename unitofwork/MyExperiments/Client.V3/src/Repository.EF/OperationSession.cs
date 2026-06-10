using Core;
using System.Data;
using Z.Dapper.Plus;

namespace Repository.EF
{
    public interface IOperationSession
    {
        IClientRepository ClientRepository { get; }
        void SetConnection(IDbConnection conn);
        void Dispose();
    }
    public class OperationSession : IOperationSession, IContextProvider
    {
        private readonly IClientUnitOfWork _unitOfWork = null;
        private IClientRepository _clientRepository;

        public OperationSession(IClientUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IClientRepository ClientRepository { get { _clientRepository = _unitOfWork.ClientRepository; return _clientRepository; } }
        public void SetConnection(IDbConnection conn)
        {
            _unitOfWork.SetConnection(conn);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
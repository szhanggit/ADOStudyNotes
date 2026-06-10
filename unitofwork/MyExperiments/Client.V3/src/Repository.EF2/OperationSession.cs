using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF2
{
    public interface IOperationSession
    {
        IMediaRepository MediaRepository { get; }
        void SetConnection(string connectionString);
        void Dispose();
    }
    public class OperationSession : IOperationSession
    {
        private IMediaUnitOfWork _unitOfWork;
        private IMediaRepository _mediaRepository;

        public OperationSession(IMediaUnitOfWork unitOfWork,
                IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public IMediaRepository MediaRepository { get { _mediaRepository = _unitOfWork.MediaRepository; return _mediaRepository; } }
        public void SetConnection(string connectionString)
        {
            _unitOfWork.SetConnection(connectionString);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}

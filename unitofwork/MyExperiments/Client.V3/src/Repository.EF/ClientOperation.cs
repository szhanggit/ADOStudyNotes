using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF
{
    public interface IClientOperation
    {
        Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection);
    }
    public class ClientOperation : IClientOperation
    {
        private readonly IOperationSession _operationSession;
        public ClientOperation(IOperationSession operationSession)
        {
            _operationSession = operationSession;
        }

        public async Task<int> CheckClientIdAsync(int ClientId, IDbConnection _dbConnection)
        {
            Client _client = null;
            try
            {
                _operationSession.SetConnection(_dbConnection);
                _client = await _operationSession.ClientRepository.GetClientByIdAsync(ClientId);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _operationSession.Dispose();
            }


            if (_client != null)
                return 1;
            else
                return 0;
        }
    }
}

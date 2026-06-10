using Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EF
{
    public interface IClientUnitOfWork : IUnitOfWork
    {
        IClientRepository ClientRepository { get; }
        ClientContext Context { get; }
    }
    public class ClientUnitOfWork : IClientUnitOfWork
    {
        private readonly ClientContext _context;
        public ClientUnitOfWork(ClientContext context,
            IClientRepository clientRepository)
        {
            _context = context;
            ClientRepository = clientRepository;
        }
        public IClientRepository ClientRepository { get; private set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public ClientContext Context
        {
            get { return _context; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SetConnection(IDbConnection conn) => _context.SetConnection(conn);


        public IDbConnection Connection => throw new NotImplementedException();
        public IDbTransaction Transaction => throw new NotImplementedException();
        public void Begin()
        { 
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public Task BeginAsync()
        {
            throw new NotImplementedException();
        }

        public Task RollbackAsync()
        {
            throw new NotImplementedException();
        }
    }
}

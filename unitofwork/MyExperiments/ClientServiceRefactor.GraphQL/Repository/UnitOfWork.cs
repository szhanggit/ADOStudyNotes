using Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IClientUnitOfWork : IUnitOfWork
    {
        IClientRepository ClientRepository { get; }
        IAddressRepository AddressRepository { get; }

        Context Context { get; }
    }
    public class UnitOfWork : IClientUnitOfWork
    {
        private readonly Context _context;
        private DbConnection _connection;
        private DbTransaction _transaction;
        public UnitOfWork(Context context)
        {
            _context = context;
            ClientRepository = new ClientRepository(_context);
            AddressRepository = new AddressRepository(_context);
        }

        public IClientRepository ClientRepository { get; private set; }
        public IAddressRepository AddressRepository { get; private set; }
        public Context Context => _context;

        public async Task<int> Complete()
        {
            if (_context.Transaction == null)
                throw new ArgumentNullException(nameof(_context.Transaction));

            _context.Transaction.Complete();
            return await Task.FromResult(0);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SetConnection(IDbConnection conn)
        {
            _context.SetConnection(conn);
        }

        public IDbConnection Connection => _connection;

        public IDbTransaction Transaction => _transaction;

        public void Begin()
        {
            _transaction = _context.Connection.BeginTransaction() as DbTransaction;
        }

        public async Task BeginAsync()
        {
            _connection = _context.Connection as DbConnection;
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            _transaction = await _connection.BeginTransactionAsync();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public async Task<int> CommitAsync()
        {
            await _transaction.CommitAsync();
            return 1;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}

using Core;
using System.Data;
using System.Data.Common;

namespace Repository.Dapper
{
    //public interface IUnitOfWork : IDisposable
    //{
    //    IDbConnection Connection { get; }
    //    IDbTransaction Transaction { get; }
    //    void Begin();
    //    void Commit();
    //    void Rollback();
    //    Task BeginAsync();
    //    Task CommitAsync();
    //    Task RollbackAsync();
    //    void SetConnection(IDbConnection conn);
    //}
    internal class UnitOfWork : IUnitOfWork
    {
        private DbConnection _connection;
        private DbTransaction _transaction;
        public UnitOfWork()
        {
            //_connection = connection;
        }
        public void SetConnection(IDbConnection conn)
        {
            _connection = conn as DbConnection;
        }
        public IDbConnection Connection => _connection;

        public IDbTransaction Transaction => _transaction;

        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }

        public async Task BeginAsync()
        {
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

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();

            _transaction = null;
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

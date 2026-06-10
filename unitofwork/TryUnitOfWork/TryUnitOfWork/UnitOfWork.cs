using System.Data;
using System.Data.SqlClient;

namespace TryUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction transaction;

        public UnitOfWork(IDbConnection connection)
        {
            transaction = connection.BeginTransaction();
        }

        public IDbTransaction Transaction => transaction;

        public void Commit()
        {
            try
            {
                transaction.Commit();
                transaction.Connection?.Close();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction?.Dispose();
                transaction.Connection?.Dispose();
                transaction = null;
            }
        }

        public void Rollback()
        {
            try
            {
                transaction.Rollback();
                transaction.Connection?.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                transaction?.Dispose();
                transaction.Connection?.Dispose();
                transaction = null;
            }
        }
    }
}

using System.Data;

namespace TryUnitOfWork
{
    public interface IUnitOfWork
    {
        IDbTransaction Transaction { get; }

        void Commit();
        void Rollback();
    }
}

using System.Data;

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Set's the connection by string
        /// </summary>
        /// <param name="connectionString"></param>
        void SetConnection(string connectionString);
        /// <summary>
        /// Set's the connection by IDbConnection i.e. new SqlConnection()
        /// </summary>
        /// <param name="connection"></param>
        void SetConnection(IDbConnection connection);
        /// <summary>
        /// Use this implementation to implement transaction commit or any similar operation like in EF SaveChangesAsync().
        /// </summary>
        /// <returns></returns>
        Task<int> Complete();
    }
}

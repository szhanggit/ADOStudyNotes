using System.Data;

namespace Core
{
    public interface IContextProvider
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginTransaction
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        /*
        public static void DeleteTransFromOlapDB(long startId, long endId)
        {
            using (var connection = new SqlConnection(SqlHelper.MoveOlapConnectionString))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();
                try
                {
                    //delete fee first then delete transaction

                    string sql = string.Format(@"Delete dbo.[TransactionFee] where TransactionId BETWEEN {0} AND {1};
                                                 Delete dbo.[Transaction] where Id BETWEEN {0} AND {1}", startId, endId);

                    using (SqlCommand cmd = new SqlCommand(sql, connection, tran))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }         
         */
    }
}

using SqlBulkTools;
using SqlBulkTools.Enumeration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BulkInsert0
{
    public class DataProviderBulkTools
    {
        public static void BatchTransactionBulkUpdate(List<BatchTransactionDetail> datailList, BatchTransactionTask task)
        {
            var bulk = new BulkOperations();

            using (TransactionScope trans = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
                {
                    bulk.Setup<BatchTransactionDetail>()
                        .ForCollection(datailList)
                        .WithTable("BatchTransactionDetail")
                        .AddColumn(x => x.ResponseCode)
                        .AddColumn(x => x.Comment)
                        .AddColumn(x => x.TranCode)
                        .BulkUpdate()
                        .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                        .MatchTargetOn(x => x.Id)
                        .Commit(conn);

                    bulk.Setup<BatchTransactionTask>()
                        .ForObject(task)
                        .WithTable("BatchTransactionTask")
                        .AddColumn(x => x.ExecuteEndTime)
                        .AddColumn(x => x.FailVoucherCount)
                        .AddColumn(x => x.SuccessVoucherCount)
                        .AddColumn(x => x.Status)
                        .Update()
                        .Where(x => x.Id == task.Id)
                        .Commit(conn);

                }

                trans.Complete();
            }
        }
    }
}

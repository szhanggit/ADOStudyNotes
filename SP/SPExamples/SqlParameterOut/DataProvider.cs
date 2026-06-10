using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SqlParameterOut
{
    public static class DataProvider
    {
        public static int PrePartialPublish(int orderLineId, int stockCount)
        {
            DataTable dt = new DataTable();
            SqlParameter publishCountParam = new SqlParameter("@publishCount", 0) { Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int };

            using (SqlDataAdapter adapter = new SqlDataAdapter("spPrePartialPublish", SqlHelper.MOConnectionString))
            {
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@orderLineId", orderLineId));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@stockCount", stockCount));
                adapter.SelectCommand.Parameters.Add(publishCountParam);
                adapter.SelectCommand.CommandTimeout = 180;
                adapter.Fill(dt);
            }

            int publishCount = (int)publishCountParam.Value;
            if (publishCount < 0)
            {
                int rowIndex = 0;
                int rowCount = dt.Rows.Count;
                int remainCount = stockCount;
                int orderLineStartSN = 0;
                int orderLineEndSN = 0;
                int beneficiaryEndSN = 0;
                int beneficiaryNextSN = 0;
                List<int> rangeList = new List<int>();

                beneficiaryEndSN = Convert.ToInt32(dt.Rows[rowIndex]["OrderBeneficiaryInfoSN"]);

                while (remainCount > 0 && rowIndex < rowCount)
                {
                    rowIndex++;

                    if (rowIndex >= rowCount)
                    {
                        beneficiaryNextSN = 1;
                    }
                    else
                    {
                        beneficiaryNextSN = Convert.ToInt32(dt.Rows[rowIndex]["OrderBeneficiaryInfoSN"]);
                    }

                    if (beneficiaryNextSN <= beneficiaryEndSN)//beneficiary结束
                    {
                        if (remainCount >= beneficiaryEndSN)//券足够发该Beneficiary
                        {
                            orderLineEndSN = Convert.ToInt32(dt.Rows[rowIndex - 1]["OrderLineSN"]);
                            orderLineStartSN = orderLineEndSN - beneficiaryEndSN + 1;

                            if (rangeList.Count > 0 && rangeList.Last() + 1 == orderLineStartSN)
                            {
                                //和上一个Beneficiary的编号连续，放在同一个range
                                rangeList[rangeList.Count - 1] = orderLineEndSN;
                            }
                            else
                            {
                                rangeList.Add(orderLineStartSN);
                                rangeList.Add(orderLineEndSN);
                            }

                            remainCount = remainCount - beneficiaryEndSN;
                        }
                    }

                    beneficiaryEndSN = beneficiaryNextSN;
                }

                //2.批量插入
                DataTable VoucherOrderBatch = new DataTable("VoucherOrderBatch");
                VoucherOrderBatch.Columns.Add("OrderId", typeof(int));
                VoucherOrderBatch.Columns.Add("OrderLineId", typeof(int));
                VoucherOrderBatch.Columns.Add("OrderLineStartSN", typeof(int));
                VoucherOrderBatch.Columns.Add("OrderLineEndSN", typeof(int));
                VoucherOrderBatch.Columns.Add("Assigned", typeof(bool));
                VoucherOrderBatch.Columns.Add("Moved", typeof(bool));
                VoucherOrderBatch.Columns.Add("SessionId", typeof(string));

                int orderId = Convert.ToInt32(dt.Rows[0]["OrderId"]);
                string sessionId = dt.Rows[0]["SessionId"].ToString();

                for (int i = 0; i < rangeList.Count; i = i + 2)
                {
                    VoucherOrderBatch.Rows.Add(orderId, orderLineId, rangeList[i], rangeList[i + 1], false, false, sessionId);
                }

                DataTableToSqlServer(VoucherOrderBatch);
                publishCount = stockCount - remainCount;
            }

            return publishCount;
        }

        public static void DataTableToSqlServer(DataTable dataTable, int timeout = 180)
        {
            if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Columns.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = dataTable.TableName;

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.BulkCopyTimeout = timeout;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}

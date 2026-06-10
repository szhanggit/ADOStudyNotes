using ADOAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MO_Execute0
{
    public static class DataProvider
    {
        public static void UpdateVoucherComboStatus(int voucherComboId, int status, string unComboby)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                conn.MO_Execute(SqlStringManager.UpdateVoucherComboStatus, new { VoucherComboId = voucherComboId, Status = status, UnComboby = unComboby });
            }
        }

        public static bool UpdateDiveTask(DiveTask task)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                bool success = false; 
                var ret = conn.MO_Execute(SqlStringManager.UpdateDiveTask, new { id = task.Id, status = task.Status, startTime = task.ExecuteStartTime });
                success = ret > 0;
                return success;
            }
        }

        public static void InsertVoucherCombo(string masterVoucherNumber, string masterProgramCode, string childVoucherNumber, string programCode, string masterRedemptionTranCode, int masterRedemptionTranAmount)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                conn.MO_Execute(SqlStringManager.InsertVoucherCombo, new
                {
                    MasterVoucherNumber = new DbString() { Value = masterVoucherNumber, IsAnsi = true },
                    MasterProgramCode = new DbString() { Value = masterProgramCode, IsAnsi = true },
                    ChildVoucherNumber = new DbString() { Value = childVoucherNumber, IsAnsi = true },
                    ProgramCode = new DbString() { Value = programCode, IsAnsi = true },
                    MasterRedemptionTranCode = new DbString() { Value = masterRedemptionTranCode, IsAnsi = true },
                    MasterRedemptionTranAmount = masterRedemptionTranAmount
                });
            }
        }
    }
}

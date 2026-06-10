using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassTable
{
    public static class DataProvider
    {
        public static void ProcessProductEmailSLMSTemplateApplyOrder(DataTable dt, DataTable SkippedList)
        {
            SqlParameter[] parameters = new SqlParameter[2] {
                new SqlParameter("@InputTable", dt),
                new SqlParameter("@SingleList", SkippedList)};
            parameters[0].SqlDbType = SqlDbType.Structured;
            parameters[0].TypeName = "ProductEmailSLMSTemplateApplyOrderType";
            parameters[1].SqlDbType = SqlDbType.Structured;
            parameters[1].TypeName = "SingleListType";
            SqlHelper.ExecuteNonQuery("spProductEmailSLMSTemplateApplyOrder", parameters);
        }
    }

    public class ProductApplyOrderLog
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public BatchStatus TaskStatus { get; set; }
        public string ProductCode { get; set; }
        public string OrderNumber { get; set; }
        public int IsEmail { get; set; }
        public int ItemStatus { get; set; }
    }

    public enum BatchStatus : byte
    {
        Running = 1,
        Fail = 2,
        PartSuccess = 4,
        Success = 8,
        Pause = 16
    }
}

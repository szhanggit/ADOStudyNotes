using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteNonQuery2
{
    public class TransactionResponse
    {
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        public string TranCode { get; set; }
        public string ServerDate { get; set; }
        public string ServerTime { get; set; }
        public string ExpireDateTime { get; set; }
        public int Balance { get; set; }
        public string Checksum { get; set; }
        public string ExternalProductCode { get; set; }
        public string ProductName { get; set; }
    }
}

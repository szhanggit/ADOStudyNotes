using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkInsert0
{
    class Program
    {
        static void Main(string[] args)
        {
            //BatchTransactionTask btt = new BatchTransactionTask {
            //    Id = 1, Status = 2, ExecuteEndTime = DateTime.Now, FailVoucherCount = 10, SuccessVoucherCount = 0
            //};

            BatchTransactionTask btt = new BatchTransactionTask
            {
                Id = 1,
                Status = 3,
                ExecuteEndTime = new DateTime(2019, 1, 29, 11, 34, 26),
                FailVoucherCount = 0,
                SuccessVoucherCount = 5
            };

            //List<BatchTransactionDetail> btdList = new List<BatchTransactionDetail> {
            //    new BatchTransactionDetail{ Id = 1, ProgramCode = "00001", VoucherNumber = "TX633162414203738", TranType = 404, Amount = 0, ResponseCode = "9999", TranCode = "TranCode1", Comment = "99991" },
            //    new BatchTransactionDetail{ Id = 2, ProgramCode = "00001", VoucherNumber = "TX451146907124982", TranType = 404, Amount = 0, ResponseCode = "9999", TranCode = "TranCode2", Comment = "99992" },
            //    new BatchTransactionDetail{ Id = 3, ProgramCode = "00001", VoucherNumber = "TX578124361273996", TranType = 404, Amount = 0, ResponseCode = "9999", TranCode = "TranCode3", Comment = "99993" },
            //    new BatchTransactionDetail{ Id = 4, ProgramCode = "00001", VoucherNumber = "TX801425981759982", TranType = 404, Amount = 0, ResponseCode = "9999", TranCode = "TranCode4", Comment = "99994" },
            //    new BatchTransactionDetail{ Id = 5, ProgramCode = "00001", VoucherNumber = "TX844415438762053", TranType = 404, Amount = 0, ResponseCode = "9999", TranCode = "TranCode5", Comment = "99995" },
            //};

            List<BatchTransactionDetail> btdList = new List<BatchTransactionDetail> {
                new BatchTransactionDetail{ Id = 1, ProgramCode = "00001", VoucherNumber = "TX633162414203738", TranType = 404, Amount = 0, ResponseCode = "0000", TranCode = null, Comment = null },
                new BatchTransactionDetail{ Id = 2, ProgramCode = "00001", VoucherNumber = "TX451146907124982", TranType = 404, Amount = 0, ResponseCode = "0000", TranCode = null, Comment = null },
                new BatchTransactionDetail{ Id = 3, ProgramCode = "00001", VoucherNumber = "TX578124361273996", TranType = 404, Amount = 0, ResponseCode = "0000", TranCode = null, Comment = null },
                new BatchTransactionDetail{ Id = 4, ProgramCode = "00001", VoucherNumber = "TX801425981759982", TranType = 404, Amount = 0, ResponseCode = "0000", TranCode = null, Comment = null },
                new BatchTransactionDetail{ Id = 5, ProgramCode = "00001", VoucherNumber = "TX844415438762053", TranType = 404, Amount = 0, ResponseCode = "0000", TranCode = null, Comment = null },
            };

            DataProviderBulkTools.BatchTransactionBulkUpdate(btdList, btt);
        }
    }
}

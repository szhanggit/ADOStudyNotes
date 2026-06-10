using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_ExecuteTrans
{
    public static class DataProvider
    {
        public static void CreateResendVoucher(int batchId, string batchNumber)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                conn.MO_Execute(@"BEGIN TRY
                                BEGIN TRAN
	                                INSERT dbo.CSSResendBatchVoucher(BatchId,OrderNumber,VoucherId,VoucherNumber,Email,Mobile,NewEmail,NewMobile,ActionType,ResponeCode,ResponeMessage)
		                                SELECT DISTINCT b.Id, b.OrderNumber, v.Id, v.VoucherNumber, bb.Email, bb.Mobile, bb.NewEmail, bb.NewMobile, bb.ActionType,
                                                IIF(vc.Id IS NOT NULL OR p.MultipleSelectionType = 2, 
													'0007', 
													CASE v.[Status] WHEN 4 THEN '0004' WHEN 8 THEN '0005' WHEN 32 THEN IIF(p.MultipleSelectionType = 1, NULL, '0003') ELSE NULL END
													),
												IIF(vc.Id IS NOT NULL OR p.MultipleSelectionType = 2, 
													'Child Voucher wouldn''t resend', 
													CASE v.[Status] WHEN 4 THEN 'Voucher expired' WHEN 8 THEN 'Voucher Trashed' WHEN 32 THEN IIF(p.MultipleSelectionType = 1, NULL, 'Voucher used') ELSE NULL END
													)
			                                FROM dbo.CSSResendBatch b WITH(NOLOCK) 
			                                JOIN dbo.CSSResendBatchBuffer bb WITH(NOLOCK) ON bb.BatchNumber = b.BatchNumber
			                                JOIN dbo.[Order] o WITH(NOLOCK) ON o.OrderNumber = b.OrderNumber
			                                JOIN dbo.OrderLine ol WITH(NOLOCK) ON ol.OrderId = o.Id
			                                JOIN dbo.OrderBeneficiaryInfo obi WITH(NOLOCK) ON obi.OrderLineId = ol.Id AND (obi.Email = bb.Email OR obi.Mobile = bb.Mobile)
			                                JOIN dbo.Voucher v WITH(NOLOCK) ON v.BeneficiaryInfoId = obi.Id
                                            JOIN dbo.Product p WITH(NOLOCK) ON p.Id = v.ProductId
                                            LEFT JOIN dbo.VoucherCombo vc WITH(NOLOCK) ON vc.ChildVoucherId = v.Id AND vc.Status = 1
			                                WHERE b.Id = @batchId
                                    INSERT dbo.CSSResendBatchVoucher(BatchId,OrderNumber,VoucherId,VoucherNumber,Email,Mobile,NewEmail,NewMobile,ActionType,ResponeCode,ResponeMessage)
	                                    SELECT b.Id, b.OrderNumber, NULL, NULL, bb.Email, bb.Mobile, bb.NewEmail, bb.NewMobile, bb.ActionType, '0001', 'Email or phone doesn''t exist'
		                                    FROM dbo.CSSResendBatch b WITH(NOLOCK) 
		                                    JOIN dbo.CSSResendBatchBuffer bb WITH(NOLOCK) ON bb.BatchNumber = b.BatchNumber
		                                    LEFT JOIN dbo.CSSResendBatchVoucher bv WITH(NOLOCK) ON bv.BatchId = b.Id AND (bv.Email = bb.Email OR bv.Mobile = bb.Mobile)
		                                    WHERE b.Id = @batchId AND bv.Id IS NULL
                                    UPDATE dbo.CSSResendBatchVoucher SET Comment = 'Skip' WHERE BatchId = @batchId AND ResponeCode IS NOT NULL;
	                                DELETE dbo.CSSResendBatchBuffer WHERE BatchNumber = @batchNumber;
	                                COMMIT;
                                END TRY
                                BEGIN CATCH
	                                ROLLBACK;
                                END CATCH", new { batchId, batchNumber });
            }
        }
    }
}

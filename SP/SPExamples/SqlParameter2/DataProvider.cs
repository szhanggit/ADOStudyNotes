using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParameter2
{
    public static class DataProvider
    {
        public static DataTable GetPreIssuedVoucher_Dynamic(int productId, int reservationCodeId, byte cacheNodeId, string ExtendTableName, long maxId, long minId, long initMinId)
        {
            string sql = $@"DECLARE @False BIT = 0;
                            SELECT  v.Id AS Id ,
                                    v.VoucherNumber AS VoucherNo ,
                                    v.GUID AS VoucherGuid ,
                                    pg.IdentityCode AS ProgramCode ,
                                    ext.Barcode1 AS Barcode1 ,
                                    ext.Barcode2 AS Barcode2 ,
                                    pbs.Barcode1Type AS Barcode1Type ,
                                    pbs.Barcode2Type AS Barcode2Type ,
                                    ISNULL(pbs.IsBarcode1APIEnabled, 0) AS IsBarcode1APIEnabled ,
                                    ISNULL(pbs.IsBarcode2APIEnabled, 0) AS IsBarcode2APIEnabled ,
                                    ISNULL(pbs.IsBarcode1DisplayText, 0) AS IsBarcode1DisplayText ,
                                    ISNULL(pbs.IsBarcode2DisplayText, 0) AS IsBarcode2DisplayText ,
                                    v.ShortUrl VoucherAlias,
                                    v.AuthCode,
                                    v.ExpiryDate,
                                    vrb.AvailableEndDate,
                                    v.PinCode,
		                            v.VoucherNumber AS Showcode,
		                            ISNULL(pbs.ShowcodeType,'') AS ShowcodeType,
		                            ISNULL(pbs.[IsShowcodeTypeDisplayText], @False) AS IsShowcodeDisplayText,
		                            ISNULL(pbs.[IsShowcodeTypeEnabled], @False) AS IsShowcodeAPIEnabled
                            FROM    dbo.Voucher v with(readpast)
                                    JOIN dbo.ProductBarcodeSetting pbs WITH(NOLOCK) ON v.ProductId = pbs.Id
                                    JOIN dbo.VoucherReservationBatch vrb WITH(NOLOCK) ON v.ReservationBatchId = vrb.id
                                    JOIN dbo.Product p WITH(NOLOCK) ON v.ProductId = p.Id
                                    JOIN dbo.Program pg WITH(NOLOCK) ON p.ProgramId = pg.Id
                                    JOIN {ExtendTableName} ext WITH(NOLOCK) ON v.ExtendId = ext.Id
                            WHERE   v.ProductId = @ProductId
                                    AND (v.Id > @MaxVoucherId)
                                    AND v.CacheNode = @CacheNodeId
                                    AND v.Status = 256
                                    AND v.BeneficiaryInfoId IS NULL
                                    AND p.Status = 1
                                    AND vrb.VoucherReservationCodeId = @ReservationCodeId";

            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, SqlHelper.MOConnectionString))
            {
                adapter.SelectCommand.CommandTimeout = 180;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@ProductId", productId));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@ReservationCodeId", reservationCodeId));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@CacheNodeId", cacheNodeId));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@MaxVoucherId", maxId));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@MinVoucherId", minId));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@InitMinVoucherId", initMinId));
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}

using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseSqlDataAdapter
{
    public static class DataProvider
    {
        public static DataTable GetComboInfoByMasterVoucher(long masterVoucherId)
        {
            DataTable dt = new DataTable();

            string sql = @"SELECT pcs.ChildProductId, 
                            pcs.ChildProductVersionId,
                            cq.BusinessTypeId, 
                            d.Name as DisplayName,
                            cqpc.ExpirationDate,
                            cqpc.[Sequence]
                            FROM [Voucher] v WITH(NOLOCK)
                                    LEFT JOIN OrderBeneficiaryInfo obfi WITH(NOLOCK) ON v.BeneficiaryInfoId = obfi.Id
                                    LEFT JOIN OrderLine ol WITH(NOLOCK) ON obfi.OrderLineId = ol.Id
                                    LEFT JOIN ClientQuotationProduct cqp WITH(NOLOCK) ON cqp.id = ol.ClientQuotationProductId
                                    LEFT JOIN ClientQuotation cq WITH(NOLOCK) ON cq.Id = cqp.ClientQuotationId
                                    LEFT JOIN ClientQuotationProductCombo cqpc WITH(NOLOCK) ON cqpc.ClientQuotationProductId = cqp.Id
                                    LEFT JOIN ExpirationPolicy ep WITH(NOLOCK) ON cqpc.ExpirySchemeId = ep.Id
                                    LEFT JOIN Dictionary d WITH(NOLOCK) ON ep.DisplayNameId = d.Id
                                    LEFT JOIN ProductComboSet pcs WITH(NOLOCK) ON cqpc.ProductComboSetId = pcs.Id
                                    INNER JOIN Product p WITH(NOLOCK) ON p.Id = pcs.ChildProductId
                            WHERE v.Id = @masterVoucherId ";

            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, SqlHelper.MOConnectionString))
            {
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@masterVoucherId", masterVoucherId));
                adapter.SelectCommand.CommandTimeout = 180;
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}

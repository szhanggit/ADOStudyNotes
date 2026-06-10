using ADOAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_ExecuteScalarSP
{
    public static class DataProvider
    {
        public static bool CheckQuotationForTrustAccount(int quotationId)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return conn.MO_ExecuteScalar<bool>("spCheckQuotationForTrustAccount", new { QuotationId = quotationId }, commandType: CommandType.StoredProcedure);
            }
        }

        public static string InsertClientPayment(List<PaymentEntity> paymentList)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            stringBuilder.AppendLine("BEGIN TRY");
            stringBuilder.AppendLine("BEGIN TRAN");
            foreach (var item in paymentList)
            {
                stringBuilder.AppendFormat("INSERT dbo.ClientPayment(ClientId,PaymentAmount,RefNumber,PaidDate) VALUES({0},{1},'{2}','{3}')", item.ClientId, item.PaymentAmount, item.ReferenceNumber, item.PaidDate);
                stringBuilder.AppendLine();
                stringBuilder.AppendFormat("IF EXISTS (SELECT 1 FROM dbo.ClientCredit WITH(NOLOCK) WHERE ClientId = {0})", item.ClientId);
                stringBuilder.AppendLine();
                stringBuilder.AppendFormat("  UPDATE dbo.ClientCredit SET AvailableCredit = AvailableCredit + {0}, MailSendStatus = 0, ModifyVersion = ModifyVersion + 1 WHERE ClientId = {1}", item.PaymentAmount, item.ClientId);
                stringBuilder.AppendLine();
            }
            stringBuilder.AppendLine("  COMMIT;");
            stringBuilder.AppendLine("  SELECT '';");
            stringBuilder.AppendLine("END TRY");
            stringBuilder.AppendLine("BEGIN CATCH");
            stringBuilder.AppendLine("  ROLLBACK");
            stringBuilder.AppendLine("  SELECT ERROR_MESSAGE();");
            stringBuilder.AppendLine("END CATCH");

            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return conn.MO_ExecuteScalar<string>(stringBuilder.ToString());
            }
        }


        public static int SaveQuotationTemplate(QuotationTemplateVersionSet qtvs)
        {
            using (var connecton = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var parameters = new DynamicParameters();
                StringBuilder sb = new StringBuilder(512);
                sb.AppendLine("BEGIN TRY;");
                sb.AppendLine("BEGIN TRAN;");
                sb.AppendLine("DECLARE @id INT = NULL;");
                sb.AppendLine("SELECT @id = Id FROM dbo.QuotationTemplateVersionSet WHERE ClientQuotationId = @ClientQuotationId AND LanguageId = @LanguageId AND IsEmail = @IsEmail AND TemplateType = @TemplateType;");
                sb.AppendLine("IF @id IS NULL OR @ClientQuotationId = 0");
                sb.AppendLine("BEGIN");
                sb.AppendLine("    INSERT dbo.QuotationTemplateVersionSet(ClientQuotationId,LanguageId,TemplateId,TemplateVersionId,TemplateType,IsEmail) VALUES(@ClientQuotationId,@LanguageId,@TemplateId,@TemplateVersionId,@TemplateType,@IsEmail);");
                sb.AppendLine("    SET @id = SCOPE_IDENTITY();");
                sb.AppendLine("END");
                sb.AppendLine("ELSE");
                sb.AppendLine("BEGIN");
                sb.AppendLine("    UPDATE dbo.QuotationTemplateVersionSet SET TemplateId = @TemplateId, TemplateVersionId = @TemplateVersionId WHERE Id = @id;");
                sb.AppendLine("    DELETE dbo.QuotationContentTagValueSet WHERE QuotationTemplateVersionSetId = @id;");
                sb.AppendLine("END");
                foreach (var item in qtvs.QuotationContentTagValueSets)
                {
                    string p1 = "@V" + parameters.ParameterNames.Count();
                    string p2 = "@T" + parameters.ParameterNames.Count();
                    parameters.Add(p1, item.Value);
                    parameters.Add(p2, item.TextValue);

                    sb.AppendFormat("INSERT dbo.QuotationContentTagValueSet(QuotationTemplateVersionSetId,ContentTagId,[Value],TextValue) VALUES(@id,{0},{1},{2});", item.ContentTagId, p1, p2);
                    sb.AppendLine();
                }
                sb.AppendLine("COMMIT;");
                sb.AppendLine("SELECT @id;");
                sb.AppendLine("END TRY");
                sb.AppendLine("BEGIN CATCH");
                sb.AppendLine("    ROLLBACK;");
                sb.AppendLine("    THROW;");
                sb.AppendLine("END CATCH");

                parameters.Add("ClientQuotationId", qtvs.ClientQuotationId);
                parameters.Add("LanguageId", qtvs.LanguageId);
                parameters.Add("TemplateId", qtvs.TemplateId);
                parameters.Add("TemplateVersionId", qtvs.TemplateVersionId);
                parameters.Add("IsEmail", qtvs.IsEmail);
                parameters.Add("TemplateType", qtvs.TemplateType);

                return connecton.MO_ExecuteScalar<int>(sb.ToString(), parameters);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOAccess
{
    public class SqlStringManager
    {
        public const string QueryStockInfo = "select VoucherStockTableName,ShortUrlPrefix from ProgramVoucherRoute where ProgramId = @programId";
        public const string QueryStockInfoWithProgramCode = "select pvr.VoucherStockTableName, pvr.ShortUrlPrefix from ProgramVoucherRoute pvr with(nolock) inner join Program p with(nolock) on pvr.ProgramId = p.Id where p.IdentityCode = @ProgramCode";
        public static string UpdateVoucherStatusForClearCache(
            string Table,
            string ProductId,
            string AvailableStartDate,
            string AvailableEndDate,
            string ExpiryDate,
            string TaskId,
            string ExcludeVoucherNumberListStr)
        {
            string InsertVoucherUpdateBufferScript = string.Empty;
            string PrepareDeleteVoucherScript = string.Empty;
            string UpdateStockScript = string.Empty;

            string CreateTempTable = $"	CREATE TABLE #TempVoucher ([VoucherNumber] varchar(100) null, [ProgramId] int null, [SessionId] varchar(100) null);";

            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(ExpiryDate))
            {
                InsertVoucherUpdateBufferScript = $"insert into VoucherUpdateBuffer (ProgramId, VoucherNumber, TaskId) " +
                    $"select v.ProgramId, v.VoucherNumber, '{TaskId}' as TaskId from Voucher v with(nolock) " +
                    $"inner join VoucherReservationBatch vrb with(nolock) on v.ReservationBatchId = vrb.Id " +
                    $"inner join Product p with(nolock) on v.ProductId = p.Id " +
                    $"where v.VoucherNumber not in ({ExcludeVoucherNumberListStr}) AND v.ProductId = '{ProductId}' AND v.[Status] = 256 AND vrb.AvailableStartDate = '{AvailableStartDate}' " +
                    $"AND vrb.AvailableEndDate = '{AvailableEndDate}' AND (case when p.RequireExpiryDate = 1 then v.ExpiryDate else vrb.ExpiryDate end is null OR case when p.RequireExpiryDate = 1 then v.ExpiryDate else vrb.ExpiryDate end > '2999-01-01');";
                PrepareDeleteVoucherScript = $"insert into #TempVoucher (VoucherNumber, ProgramId) select v.VoucherNumber, v.ProgramId from Voucher v with(nolock) " +
                    $"inner join VoucherReservationBatch vrb with(nolock) on v.ReservationBatchId = vrb.Id inner join Product p with(nolock) on v.ProductId = p.Id " +
                    $"where v.VoucherNumber not in ({ExcludeVoucherNumberListStr}) AND v.ProductId = '{ProductId}' AND v.[Status] = 256 AND vrb.AvailableStartDate = '{AvailableStartDate}' " +
                    $"AND vrb.AvailableEndDate = '{AvailableEndDate}' AND (case when p.RequireExpiryDate = 1 then v.ExpiryDate else vrb.ExpiryDate end is null OR case when p.RequireExpiryDate = 1 then v.ExpiryDate else vrb.ExpiryDate end > '2999-01-01');";
                UpdateStockScript = $"update stock set stock.StockStatus = 0 from {Table} stock with(nolock) " +
                    $"inner join VoucherReservationBatch vrb with(nolock) on stock.ReservationBatchId = vrb.Id " +
                    $"inner join VoucherUpdateBuffer buf with(nolock) on stock.VoucherNumber = buf.VoucherNumber " +
                    $"AND stock.ProgramId = buf.ProgramId inner join Product p with(nolock) on stock.ProductId = p.Id where stock.ProductId = '{ProductId}' AND stock.StockStatus = 1 " +
                    $"AND vrb.AvailableStartDate = '{AvailableStartDate}' AND vrb.AvailableEndDate = '{AvailableEndDate}' " +
                    $"AND (case when p.RequireExpiryDate = 1 then stock.ExpiryDate else vrb.ExpiryDate end is null OR case when p.RequireExpiryDate = 1 then stock.ExpiryDate else vrb.ExpiryDate end > '2999-01-01') AND buf.TaskId = '{TaskId}';";
            }
            else
            {
                InsertVoucherUpdateBufferScript = $"insert into VoucherUpdateBuffer (ProgramId, VoucherNumber, TaskId) " +
                    $"select v.ProgramId, v.VoucherNumber, '{TaskId}' as TaskId from Voucher v with(nolock) " +
                    $"inner join VoucherReservationBatch vrb with(nolock) on v.ReservationBatchId = vrb.Id " +
                    $"inner join Product p with(nolock) on v.ProductId = p.Id " +
                    $"where v.VoucherNumber not in ({ExcludeVoucherNumberListStr}) AND v.ProductId = '{ProductId}' AND v.[Status] = 256 AND vrb.AvailableStartDate = '{AvailableStartDate}' " +
                    $"AND vrb.AvailableEndDate = '{AvailableEndDate}' AND case when p.RequireExpiryDate = 1 then v.ExpiryDate else vrb.ExpiryDate end = '{ExpiryDate}';";
                PrepareDeleteVoucherScript = $"insert into #TempVoucher (VoucherNumber, ProgramId) select v.VoucherNumber, v.ProgramId from Voucher v with(nolock) " +
                    $"inner join VoucherReservationBatch vrb with(nolock) on v.ReservationBatchId = vrb.Id inner join Product p with(nolock) on v.ProductId = p.Id " +
                    $"where v.VoucherNumber not in ({ExcludeVoucherNumberListStr}) AND v.ProductId = '{ProductId}' AND v.[Status] = 256 AND vrb.AvailableStartDate = '{AvailableStartDate}' " +
                    $"AND vrb.AvailableEndDate = '{AvailableEndDate}' AND case when p.RequireExpiryDate = 1 then v.ExpiryDate else vrb.ExpiryDate end = '{ExpiryDate}';";
                UpdateStockScript = $"update stock set stock.StockStatus = 0 from {Table} stock with(nolock) " +
                    $"inner join VoucherReservationBatch vrb with(nolock) on stock.ReservationBatchId = vrb.Id " +
                    $"inner join VoucherUpdateBuffer buf with(nolock) on stock.VoucherNumber = buf.VoucherNumber " +
                    $"AND stock.ProgramId = buf.ProgramId inner join Product p with(nolock) on stock.ProductId = p.Id where stock.ProductId = '{ProductId}' AND stock.StockStatus = 1 " +
                    $"AND vrb.AvailableStartDate = '{AvailableStartDate}' AND vrb.AvailableEndDate = '{AvailableEndDate}' " +
                    $"AND case when p.RequireExpiryDate = 1 then stock.ExpiryDate else vrb.ExpiryDate end = '{ExpiryDate}' AND buf.TaskId = '{TaskId}';";
            }

            string DeleteVoucherScript = $"WHILE (1=1) " +
                $"BEGIN " +
                $"update top(1000) #TempVoucher set SessionId = '1' where SessionId is null; " +
                $"update v set v.[Status] = 64 from Voucher v with(nolock) " +
                $"inner join #TempVoucher t with(nolock) on v.VoucherNumber = t.VoucherNumber AND v.ProgramId = t.ProgramId " +
                $"where t.SessionId = '1'; " +
                $"update #TempVoucher set SessionId = '2' where SessionId = '1'; " +
                $"IF(@@ROWCOUNT = 0) " +
                $"BREAK;  " +
                $"END";

            string DeleteVoucherTrustAccountBuffer = $"delete vtbuf from VoucherUpdateBuffer buf with(nolock) inner join Voucher v with(nolock) " +
                $"on buf.ProgramId = v.ProgramId AND buf.VoucherNumber = v.VoucherNumber inner join VoucherTrustAccountBuffer vtbuf with(nolock) " +
                $"on vtbuf.VoucherId = v.Id where buf.TaskId = '{TaskId}';";
            string DeleteVoucherUpdateBufferScript = $"delete from VoucherUpdateBuffer where TaskId = '{TaskId}';";
            sb.AppendLine(CreateTempTable);
            sb.AppendLine(InsertVoucherUpdateBufferScript);
            sb.AppendLine(PrepareDeleteVoucherScript);
            sb.AppendLine(DeleteVoucherScript);
            sb.AppendLine(UpdateStockScript);
            sb.AppendLine(DeleteVoucherTrustAccountBuffer);
            sb.AppendLine(DeleteVoucherUpdateBufferScript);
            string result = sb.ToString();
            string OutWrapper = $"BEGIN TRAN    " +
                $"BEGIN try   {result}    " +
                $"COMMIT TRAN;    " +
                $"select 0    " +
                $"END try  " +
                $"begin catch " +
                $"ROLLBACK TRAN   " +
                $"select - 1  " +
                $"end catch   ";
            return OutWrapper;
        }


        public const string UpdateVoucherComboStatus = @"UPDATE vouchercombo
                                                                  SET 
                                                                      [status] = @status, 
                                                                      [UncomboOn] = GETDATE(), 
                                                                      [UncomboBy] = @unComboby
                                                                WHERE Id = @voucherComboId ";

        public const string UpdateDiveTask = "update DiveTask set [Status] = @status, ExecuteStartTime = @startTime, ExecuteEndTime = getdate() where Id = @id";

        public const string InsertVoucherCombo = @"INSERT  INTO dbo.VoucherCombo
                                                            ( MasterVoucherId ,
                                                              ChildVoucherId ,
                                                              Status ,
                                                              ComboOn ,
                                                              MasterRedemptionTranCode,
                                                              MasterRedemptionTranAmount
                                                            )
                                                    VALUES  ( ( SELECT  t1.Id as MasterVoucherId
                                                                FROM    dbo.Voucher AS t1 WITH ( NOLOCK )
                                                                        JOIN dbo.Program AS t2 WITH ( NOLOCK ) ON t2.Id = t1.ProgramId
                                                                WHERE   VoucherNumber = @MasterVoucherNumber
                                                                        AND t2.IdentityCode = @MasterProgramCode
                                                              ) ,
                                                              ( SELECT  t1.Id as ChildVoucherId
                                                                FROM    dbo.Voucher AS t1 WITH ( NOLOCK )
                                                                        JOIN dbo.Program AS t2 WITH ( NOLOCK ) ON t2.Id = t1.ProgramId
                                                                WHERE   VoucherNumber = @ChildVoucherNumber
                                                                        AND t2.IdentityCode = @ProgramCode
                                                              ) ,
                                                              1 ,
                                                              GETDATE(),
                                                              @MasterRedemptionTranCode,
                                                              @MasterRedemptionTranAmount
                                                            )";

        public static string TryInsertCampaignVouchersFromStock(string tableName, string prefix)
        {
            return string.Format(@"declare @step int = 3000;
                                    while 1 > 0
                                    begin
	                                    begin try
	                                    begin tran
	                                    insert into Voucher(ProgramId,VoucherNumber,ProductId,BalanceAvailable,[STATUS],ExpiryDate,[GUID],ReservationBatchId,CacheNode,ShortUrl,AuthCode,ExtendId,VSessionId,PinCode,BeneficiaryInfoId) 
		                                    select temp.ProgramId,temp.VoucherNumber,temp.ProductId,temp.BalanceAvailable,IIF(@cacheNode IS NOT NULL, 128, 64),temp.ExpiryDate,temp.[GUID],
		                                    temp.ReservationBatchId,@cacheNode,temp.ShortUrl,IIF(@needChangeAuthCode = 1, right(temp.voucherNumber,4), temp.AuthCode),{1} + temp.Id, @sessionId, temp.PinCode,@orderBeneficiaryInfoId from 
		                                    (select top (@step) Id, ProgramId,VoucherNumber,ProductId,BalanceAvailable,ExpiryDate,[GUID],ReservationBatchId,ShortUrl,AuthCode,PinCode
			                                    from {0} stock with(index=[index_StockStatus_Locked],nolock) where StockStatus = 2 and SessionId = @sessionId order by Id)temp
		                                    left join Voucher v with(nolock) on v.ProgramId = temp.ProgramId and v.VoucherNumber = temp.VoucherNumber where v.Id is null

	                                    update stock set AssignTime = getdate(), StockStatus = 1
		                                    from (select top (@step) AssignTime,StockStatus,AuthCode,voucherNumber from {0} stock with(index=[index_StockStatus_Locked],nolock) where StockStatus = 2 and SessionId = @sessionId order by Id) stock

	                                    if @@ROWCOUNT = @step
		                                    COMMIT TRAN
	                                    else
	                                    begin
		                                    if exists (select 1 from {0} with(nolock) where StockStatus = 2 and SessionId = @sessionId)
		                                    begin
			                                    ROLLBACK TRAN
			                                    select -1
			                                    return;
		                                    end
		                                    else
		                                    begin
			                                    COMMIT TRAN
			                                    select 0
			                                    return;
		                                    end
	                                    end

	                                    end try
	                                    begin catch
		                                    ROLLBACK TRAN
		                                    select -1
		                                    return;
	                                    end catch
                                    end", tableName, ConvertStringToLong(prefix));
        }

        public static long ConvertStringToLong(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                char c = str.ToCharArray()[0];
                return (c - 48) * 100000000000000000;//'0'是48，'z'是122，将c映射成两位数
            }
            return 0;
        }

        public static string TrashVoucherInInventory(string tableName, string VoucherNumber)
        {
            string sql = string.Empty;
            sql = $"if exists (select * from {tableName} with(nolock) where VoucherNumber = '{VoucherNumber}') " +
                $"begin update {tableName} set StockStatus = 8 where VoucherNumber = '{VoucherNumber}'; select 1; end " +
                $"else " +
                $"begin select -100; end";
            return sql;
        }

        public static string QueryStockCountAndLock_GR(string tableName)
        {
            return string.Format(@"begin try
                                begin tran
	                                declare @sessionId varchar(50) = CONVERT(varchar(50),NEWID())
	                                declare @CurrentDate int = CAST(convert(varchar,getdate(),112) as int);
	                                declare @timecutline datetime = DATEADD(DAY, 14, getdate());

	                                update {0}  set SessionId = @sessionId,StockStatus = 2 from
	                                (select top(@expectCount) stock.Id from {0} stock with(index=index_StockStatus_Stock) join VoucherReservationBatch batch on stock.ReservationBatchId = batch.Id
		                                where stock.StockStatus = 0 and stock.ProductId = @productId and (stock.ExpiryDate >= @timecutline or stock.ExpiryDate IS NULL) and batch.VoucherReservationCodeId = @reservationCodeId
                                        and @CurrentDate >= batch.AvailableStartDate and batch.AvailableEndDate >= @CurrentDate order by stock.ExpiryDate)t
	                                where {0}.Id = t.Id

	                                declare @affectCount int = @@ROWCOUNT;
	                                if @affectCount = @expectCount or @canLessThanExpect = 1
	                                begin
                                        IF EXISTS (SELECT 1 FROM dbo.[Order] o WITH(NOLOCK) JOIN dbo.OrderLine ol WITH(NOLOCK) ON ol.OrderId = o.Id WHERE ol.Id = @OrderLineId AND o.Mode = 2)
	                                        INSERT dbo.ApiVoucherStockBook(ProductId, SessionId, TableName, LockedTime) VALUES (@productId, @sessionId, '{0}', GETDATE());
                                        INSERT dbo.VoucherStockBook(OrderLineId,SessionId,BookStatus,CreationTime) values(@OrderLineId,@sessionId,0,GETDATE());
		                                select @affectCount StockCount,@sessionId SessionId;
		                                commit tran;
	                                end
	                                else
	                                begin
		                                select 0 StockCount,'' SessionId;
		                                rollback tran;
	                                end
                                end try
                                begin catch
	                                select 0 StockCount,'' SessionId;
	                                rollback tran;
                                end catch", tableName);
        }

        public static string INImportedThirdPartyProductStat()
        {
            return @"   set nocount on;
                        BEGIN TRAN
                        BEGIN TRY
	                    declare @table table
	                    (
		                    Id int identity(1,1),
		                    TableName varchar(50)
	                    );
	                    insert @table(TableName) select VoucherStockTableName from ProgramVoucherRoute group by VoucherStockTableName

	                    declare @rowCount int = (select count(1) from @table);
	                    declare @rowIndex int = 1;
	                    declare @programId int;
	                    declare @tableName varchar(50);
	                    declare @sql nvarchar(1000);
                        
                        UPDATE ProductStat set LastModifiedON = GETDATE(),GeneratedQuantity = 0, InStockQuantity =0;
                        
	                    while(@rowIndex <= @rowCount)
	                    begin
		                    set @tableName = (select TableName from @table where Id = @rowIndex);
		                    set @rowIndex = @rowIndex + 1;

		                    set @sql = 'UPDATE ProductStat SET LastModifiedON = GETDATE(),GeneratedQuantity = t.GeneratedQuantity'
						                    + ' FROM (SELECT ps.ProductId,SUM(CASE WHEN v.ProductId IS NULL THEN 0 ELSE 1 END) AS GeneratedQuantity'
							                    + ' FROM ProductStat ps INNER JOIN ' + @tableName + ' v WITH(NOLOCK) ON v.VoucherNumber >= ps.MiniVoucherNumber AND v.VoucherNumber <= ps.MaxVoucherNumber AND LEN(v.VoucherNumber) = ps.VoucherNumberFullLength'
							                    + ' WHERE ps.VoucherNumberFullLength IS NOT NULL GROUP BY ps.ProductId) t'
						                    + ' WHERE ProductStat.ProductId = t.ProductId';
		                    exec (@sql);
		
		                    set @sql = 'UPDATE ProductStat SET LastModifiedON = GETDATE(),GeneratedQuantity = t.GeneratedQuantity'
						                    + ' FROM (SELECT ps.ProductId,SUM(CASE WHEN v.ProductId IS NULL THEN 0 ELSE 1 END) AS GeneratedQuantity'
							                    + ' FROM ProductStat ps INNER JOIN ' + @tableName + ' v WITH(NOLOCK) ON ps.ProductId = v.ProductId'
							                    + ' WHERE ps.VoucherNumberFullLength IS NULL GROUP BY ps.ProductId) t'
							                    + ' WHERE ProductStat.ProductId = t.ProductId';
		                    exec (@sql);

		                    set @sql = 'UPDATE ProductStat SET LastModifiedON = GETDATE(),InStockQuantity = t.InStockQuantity'
						                    + ' FROM (SELECT ps.ProductId,SUM(CASE WHEN v.ProductId IS NULL THEN 0 ELSE 1 END) AS InStockQuantity'
							                    + ' FROM ProductStat ps INNER JOIN ' + @tableName + ' v WITH(NOLOCK) ON ps.ProductId = v.ProductId AND v.StockStatus = 0'
							                    + ' GROUP BY ps.ProductId) t'
							                    + ' WHERE ProductStat.ProductId = t.ProductId';
		                    exec (@sql);
	                    end
                        
                        set @sql = 'UPDATE ProductStat SET LastModifiedON = GETDATE(), InCacheQuantity = t.InCacheQuantity'
								+ ' FROM (SELECT p.Id AS ProductId, SUM(CASE WHEN v.Id IS NULL THEN 0 ELSE 1 END) AS InCacheQuantity '
										+' FROM dbo.Product p with(nolock) LEFT JOIN Voucher v with(nolock) ON p.Id = v.ProductId WHERE p.VoucherNumberGenerateWay = 2 AND p.VoucherSupplierId IS NULL AND v.STATUS = 256'
										+' GROUP BY p.Id ) t'
							            +' WHERE ProductStat.ProductId = t.ProductId';
						exec (@sql);

                    END TRY
                    BEGIN CATCH
	                    SELECT ERROR_MESSAGE();
                        IF @@TRANCOUNT > 0
			                ROLLBACK TRAN
                    END CATCH
                    IF @@TRANCOUNT > 0
		            COMMIT TRAN";
        }


        public static string CheckBeneficiaryEmailOrMobile(bool isEmail, string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return string.Format(@"IF EXISTS (SELECT 1 FROM dbo.VoucherOrder WITH(NOLOCK) 
					                                    WHERE OrderLineId = @orderLineId 
					                                    AND OrderLineSN BETWEEN @OrderLineStartSN AND @OrderLineEndSN
                                                        AND VoucherId IS NOT NULL
					                                    AND {0} = 1)
	                                    SELECT 1;
                                    ELSE
	                                    SELECT 0;", isEmail ? "IsEmailDelivery" : "IsSLMSDelivery");
            }

            return string.Format(@"IF EXISTS (SELECT 1 FROM dbo.VoucherOrder vo WITH(NOLOCK)
					                                    JOIN dbo.VoucherOrderBatch b WITH(NOLOCK) ON b.OrderLineId = vo.OrderLineId AND b.OrderLineStartSN <= vo.OrderLineSN AND b.OrderLineEndSN >= vo.OrderLineSN 
					                                    WHERE vo.OrderLineId = @orderLineId AND {0} = 1 AND b.SessionId = '{1}')
	                                    SELECT 1;
                                    ELSE
	                                    SELECT 0;", isEmail ? "IsEmailDelivery" : "IsSLMSDelivery", sessionId);
        }

        public const string GetMasterVoucherAndProductByComboChildVoucher = @"SELECT v.ShortUrl, 
                                                                                       v.VoucherNumber AS CurrentVoucherNumber,
                                                                                       vc.MasterVoucherId AS currentVoucherid,
                                                                                       t1.IdentityCode AS CurrentProgramIdentityCode,
                                                                                       vc.ChildVoucherId,
                                                                                       v2.VoucherNumber AS ChildAccountNumber,
                                                                                       t2.IdentityCode AS ChildAccountProgramIdentityCode,
                                                                                       vc.MasterVoucherId,
                                                                                       v.VoucherNumber AS MasterAccountNumber,
                                                                                       t1.IdentityCode AS MasterAccountProgramIdentityCode,
                                                                                       v.ProgramId, 
                                                                                       p.ProductName, 
                                                                                       v.[Status] AS VoucherStatus, 
                                                                                       p.VoucherNumberGenerateWay, 
                                                                                       vc.MasterRedemptionTranCode, 
                                                                                       vc.MasterRedemptionTranAmount,
                                                                                       vc.id as vouchercomboid
                                                                                FROM vouchercombo vc WITH(NOLOCK)
                                                                                     INNER JOIN Voucher v WITH(NOLOCK) ON v.Id = vc.MasterVoucherId
                                                                                     INNER JOIN product p WITH(NOLOCK) ON v.ProductId = p.Id
                                                                                     INNER JOIN Program t1 WITH(NOLOCK) ON t1.Id = v.ProgramId
                                                                                     INNER JOIN Voucher v2 WITH(NOLOCK) ON v2.Id = vc.ChildVoucherId
                                                                                     INNER JOIN Program t2 WITH(NOLOCK) ON t2.Id = v2.ProgramId
                                                                                WHERE vc.childVoucherId = @childVoucherId 
                                                                                      and vc.[status] = 1 ";

        public const string IsChildVoucher = @"SELECT ISNULL(
                                                        (
                                                            SELECT top (1) 1
                                                            FROM vouchercombo vc WITH(NOLOCK)
                                                            WHERE vc.childvoucherid = @VoucherId
                                                                  AND vc.[status] = 1
                                                        ), 0) ";

        public const string GetOrderLineIds = @" SELECT ol.id as OrderLineId
                                                  FROM dbo.[EMV_ClientOrderLine] ecol WITH(NOLOCK)
                                                  JOIN dbo.[Order] o WITH(NOLOCK) on ecol.OrderNumber = o.OrderNumber
                                                  JOIN dbo.[OrderLine] ol WITH(NOLOCK) on ol.OrderId = o.Id
                                                  WHERE ecol.RCN = @RCN";

        public const string GetClientQuotationProductId = @"  select cqp.Id as ClientQuotationProductId
                                                                  from dbo.Product p with(nolock)
                                                                  join dbo.ProductVersion pv with(nolock) on p.Id = pv.ProductId
                                                                  join dbo.clientquotationproduct cqp with(nolock) on pv.Id = cqp.ProductVersionId
                                                                  join dbo.ClientQuotation cq with(nolock) on cqp.ClientQuotationId = cq.Id
                                                                  where p.ProductCode = @ProductCode 
                                                                  and cq.ProjectCode = @ProjectCode";
    }
}

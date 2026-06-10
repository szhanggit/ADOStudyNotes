IF EXISTS (
		SELECT NULL
		FROM dbo.sysobjects
		WHERE id = object_id(N'[dbo].[spSubProductEmailApplyOrder]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
	DROP PROCEDURE [dbo].[spSubProductEmailApplyOrder]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spSubProductEmailApplyOrder] @ProductCode varchar(50)
	,@OrderNumber varchar(50), @RecordId INT, @SingleList SingleListType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Id INT;
	DECLARE @TagId int;
	DECLARE @TagName varchar(50);
	DECLARE @ProductEmailTemplateTagValueId int;
	DECLARE @OrderEmailTemplateTagValueId int;
	DECLARE @ProductEmailTemplateVersionId int;
	DECLARE @OrderEmailTemplateVersionId int;
	DECLARE @OrderEmailTemplateVersionSetId int;
	DECLARE @OrderLineId int;
	DECLARE @EmailTemplateVersionId int;
  	DECLARE @BusinessCursor as CURSOR;
	DECLARE @IdCursor AS CURSOR;
	DECLARE @val nvarchar(max);
	DECLARE @IsProductNewEmailTemplate bit = 0;
	DECLARE @IsOrderNewEmailTemplate bit = 0;
	
	create table dbo.#RunningEmailResult
	(
		Id INT PRIMARY KEY IDENTITY (1, 1), 
		ProductCode VARCHAR(50),
		OrderNumber VARCHAR(50),
		TagName VARCHAR(50),
		Status VARCHAR(50),
		OperationType VARCHAR(50),
		[Type] VARCHAR(50)
	)
  
  
  
    if exists(select * from ClientQuotationProduct cqp with(nolock) 
		inner join OrderLine ol with(nolock) on ol.ClientQuotationProductId = cqp.Id
		inner join [Order] o with(nolock) on o.Id = ol.OrderId
		inner join ProductVersion pv with(nolock) on cqp.ProductVersionId = pv.Id
		inner join Product p with(nolock) on pv.ProductId = p.Id
		where p.ProductCode = @ProductCode and o.OrderNumber = @OrderNumber and ol.EmailTemplateVersionId is not null and ol.OrderEmailTemplateVersionId is null)
		begin--Old Version
			set @IsOrderNewEmailTemplate = 0;
		end
		else
		begin--New Version
			set @IsOrderNewEmailTemplate = 1;
		end
  
    if exists(select * from ClientQuotationProduct cqp with(nolock) 
		inner join OrderLine ol with(nolock) on ol.ClientQuotationProductId = cqp.Id
		inner join [Order] o with(nolock) on o.Id = ol.OrderId
		inner join ProductVersion pv with(nolock) on cqp.ProductVersionId = pv.Id
		inner join Product p with(nolock) on pv.ProductId = p.Id
		where p.ProductCode = @ProductCode and o.OrderNumber = @OrderNumber and ol.EmailTemplateVersionId is null and ol.OrderEmailTemplateVersionId is null)
		begin--Old Version
			set @IsOrderNewEmailTemplate = 0;
		end
		
	if exists(SELECT * FROM Product WITH(nolock) where ProductCode = @ProductCode and EmailTemplateVersionId is not null and ProductEmailTemplateVersionId is null)
		begin--Old Version
			set @IsProductNewEmailTemplate = 0;
		end
		else
		begin--New Version
			set @IsProductNewEmailTemplate = 1;
		end
  
  
  ---------------------------------------
  
  
  
  if @IsOrderNewEmailTemplate = 0 and @IsProductNewEmailTemplate = 0
		begin--Old Version
			select @OrderLineId = ol.Id, @EmailTemplateVersionId = ol.EmailTemplateVersionId 
			from OrderLine ol with(nolock) 
			inner join [Order] o with(nolock) on ol.OrderId = o.Id 
			inner join ClientQuotationProduct cqp with(nolock) on ol.ClientQuotationProductId = cqp.Id
			inner join ProductVersion pv on pv.Id = cqp.ProductVersionId
			inner join Product p on p.Id = pv.ProductId
			where p.ProductCode = @ProductCode and o.OrderNumber = @OrderNumber
			
			if(@OrderLineId is null or @OrderLineId = '')
			BEGIN
				UPDATE ProductEmailSLMSTemplateApplyOrderLog
				SET Status = 1
				WHERE Id = @RecordId;				
				return
			END		
		
			if(@EmailTemplateVersionId is null or @EmailTemplateVersionId = '')
			BEGIN
				UPDATE ProductEmailSLMSTemplateApplyOrderLog
				SET Status = 1
				WHERE Id = @RecordId;				
				return
			END		
		
			update OrderLine
			set EmailTemplateVersionId = @EmailTemplateVersionId
			where id = @OrderLineId
			
			BEGIN TRAN

			BEGIN TRY
				SET @BusinessCursor = CURSOR FOR
				select Id from ContentTag

				OPEN @BusinessCursor;
				FETCH NEXT FROM @BusinessCursor INTO @TagId;
				WHILE @@FETCH_STATUS = 0
				BEGIN
				-------------


				select @val = [Value] from ProductEmailContentTagValueSet pes with(nolock)
				where pes.ProductEmailTemplateTagValueId = @ProductEmailTemplateTagValueId
				and pes.ContentTagId = @TagId;


				IF @val is not null and @val <> ''  
				BEGIN		
					select @TagName = TagName from ContentTag where Id = @TagId;
					if exists(select * from OrderEmailContentTagValueSet with(nolock) where OrderLineId = @OrderLineId and ContentTagId = @TagId)
						begin
							update OrderEmailContentTagValueSet
							set [Value] = @val
							where OrderLineId = @OrderLineId and ContentTagId = @TagId
							
							insert into #RunningEmailResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, @TagName, 'Success', 'Update', 'Email');
						end
					else
						begin
							insert into OrderEmailContentTagValueSet(ContentTagId, OrderLineId, Value) 
							values (@TagId, @OrderLineId, @val);			
								
							insert into #RunningEmailResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, @TagName, 'Success', 'Insert', 'Email');
						end
				END

				set @val = '';				
				----------------
				FETCH NEXT FROM @BusinessCursor INTO @TagId;
				END
				
				
				UPDATE ProductEmailSLMSTemplateApplyOrderLog
				SET Status = 1
				WHERE Id = @RecordId;

				select ProductCode, OrderNumber, TagName, Status, OperationType, [Type] from #RunningEmailResult;	
			END TRY
			BEGIN CATCH
					ROLLBACK TRAN
					return
			END CATCH

			COMMIT TRAN					
		
		end
  else
		begin--New Version

		
			select @ProductEmailTemplateTagValueId = EmailTemplateTagValueId, @ProductEmailTemplateVersionId = EmailTemplateVersionId from ProductEmailTemplateVersionSet with(nolock)
			where ProductEmailTemplateVersionId in (select ProductEmailTemplateVersionId from Product with(nolock) where ProductCode = @ProductCode)

			if(@ProductEmailTemplateTagValueId is null or @ProductEmailTemplateTagValueId = '')
			BEGIN
				UPDATE ProductEmailSLMSTemplateApplyOrderLog
				SET Status = 1
				WHERE Id = @RecordId;				
				return
			END

			if(@ProductEmailTemplateVersionId is null or @ProductEmailTemplateVersionId = '')
			BEGIN
				UPDATE ProductEmailSLMSTemplateApplyOrderLog
				SET Status = 1
				WHERE Id = @RecordId;				
				return
			END		
		
		
		
			if @IsOrderNewEmailTemplate = 1
			begin 
				select @OrderEmailTemplateVersionSetId = Id, @OrderEmailTemplateTagValueId = EmailTemplateTagValueId, @OrderEmailTemplateVersionId = EmailTemplateVersionId from OrderEmailTemplateVersionSet with(nolock) where OrderEmailTemplateVersionId in (
					select ol.OrderEmailTemplateVersionId from ClientQuotationProduct cqp with(nolock) 
					inner join OrderLine ol with(nolock) on ol.ClientQuotationProductId = cqp.Id
					inner join [Order] o with(nolock) on o.Id = ol.OrderId
					inner join ProductVersion pv with(nolock) on cqp.ProductVersionId = pv.Id
					inner join Product p with(nolock) on pv.ProductId = p.Id
					where p.ProductCode = @ProductCode and o.OrderNumber = @OrderNumber
				) 

				if(@OrderEmailTemplateVersionSetId is null or @OrderEmailTemplateVersionSetId = '')
				BEGIN
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;					
					return
				END

				if(@OrderEmailTemplateTagValueId is null or @OrderEmailTemplateTagValueId = '')
				BEGIN
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;					
					return
				END

				if(@OrderEmailTemplateVersionId is null or @OrderEmailTemplateVersionId = '')
				BEGIN
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;					
					return
				END

				update OrderEmailTemplateVersionSet
				set EmailTemplateVersionId = @ProductEmailTemplateVersionId
				where id = @OrderEmailTemplateVersionSetId


				BEGIN TRAN

				BEGIN TRY
					SET @BusinessCursor = CURSOR FOR
					select Id from ContentTag

					OPEN @BusinessCursor;
					FETCH NEXT FROM @BusinessCursor INTO @TagId;
					WHILE @@FETCH_STATUS = 0
					BEGIN
					-------------


					select @val = [Value] from ProductEmailContentTagValueSet pes with(nolock)
					where pes.ProductEmailTemplateTagValueId = @ProductEmailTemplateTagValueId
					and pes.ContentTagId = @TagId;


					IF @val is not null and @val <> ''  
					BEGIN		
						select @TagName = TagName from ContentTag where Id = @TagId;
						if exists(select * from OrderEmailTemplateTagValueSet with(nolock) where OrderEmailTemplateTagValueId = @OrderEmailTemplateTagValueId and ContentTagId = @TagId)
							BEGIN
								IF NOT EXISTS(SELECT * FROM @SingleList WHERE Id = @TagId)
								BEGIN
									update OrderEmailTemplateTagValueSet
									set [Value] = @val
									where OrderEmailTemplateTagValueId = @OrderEmailTemplateTagValueId and ContentTagId = @TagId;
								
									insert into #RunningEmailResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, @TagName, 'Success', 'Update', 'Email');
								END
							end
						else
							begin
								insert into  OrderEmailTemplateTagValueSet(ContentTagId, OrderEmailTemplateTagValueId, Value) 
								values (@TagId, @OrderEmailTemplateTagValueId, @val);				
								
								insert into #RunningEmailResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, @TagName, 'Success', 'Insert', 'Email');
							end
					END

					set @val = '';				
					----------------
					FETCH NEXT FROM @BusinessCursor INTO @TagId;
					END
					
					
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;

					select ProductCode, OrderNumber, TagName, Status, OperationType, [Type] from #RunningEmailResult;	
				END TRY
				BEGIN CATCH
						SELECT ERROR_MESSAGE() AS [ErrorMessage];
						ROLLBACK TRAN
						insert into #RunningEmailResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, '', 'Fail', '', 'Email');
						select ProductCode, OrderNumber, TagName, Status, OperationType, [Type] from #RunningEmailResult;
						return
				END CATCH

				COMMIT TRAN			
			end
			else
			begin--It is old order template. It needs updating to the new one.
				BEGIN TRAN
				BEGIN TRY


					select @OrderEmailTemplateTagValueId = Max(Id) from OrderEmailTemplateTagValue with(nolock);
					Set @OrderEmailTemplateTagValueId = @OrderEmailTemplateTagValueId + 1;
					select @OrderEmailTemplateVersionId = Max(Id) from OrderEmailTemplateVersionSet with(nolock);
					Set @OrderEmailTemplateVersionId = @OrderEmailTemplateVersionId + 1;
			
					INSERT OrderEmailTemplateTagValue DEFAULT VALUES;
					SELECT @OrderEmailTemplateTagValueId = SCOPE_IDENTITY();
				
					INSERT OrderEmailTemplateVersion DEFAULT VALUES;
					SELECT @OrderEmailTemplateVersionId = SCOPE_IDENTITY();
				
					INSERT INTO OrderEmailTemplateTagValueSet
					SELECT pes.ContentTagId as ContentTagId, @OrderEmailTemplateTagValueId as OrderEmailTemplateTagValueId, [Value] as [Value] FROM ProductEmailContentTagValueSet pes WITH(NOLOCK) WHERE pes.ProductEmailTemplateTagValueId = @ProductEmailTemplateTagValueId;
				
					insert into OrderEmailTemplateVersionSet (OrderEmailTemplateVersionId, EmailTemplateVersionId, EmailTemplateTagValueId) values (@OrderEmailTemplateVersionId, @ProductEmailTemplateVersionId, @OrderEmailTemplateTagValueId);
				
					SELECT @OrderLineId = ol.Id FROM [Order] o WITH(NOLOCK) 
					INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId 
					INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON ol.ClientQuotationProductId = cqp.Id
					INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON pv.Id = cqp.ProductVersionId
					INNER JOIN dbo.Product p WITH(NOLOCK) ON pv.ProductId = p.Id
					WHERE o.OrderNumber = @OrderNumber AND p.ProductCode = @ProductCode				
				
					if(@OrderLineId is null or @OrderLineId = '')
					BEGIN
						UPDATE ProductEmailSLMSTemplateApplyOrderLog
						SET Status = 1
						WHERE Id = @RecordId;						
						return
					END		
			
					-----------------------------------------

					SET @IdCursor = CURSOR FOR
					select Id from @SingleList

					OPEN @IdCursor;
					FETCH NEXT FROM @IdCursor INTO @Id;
					WHILE @@FETCH_STATUS = 0
					BEGIN
					-------------

					SELECT @val = Value FROM OrderEmailContentTagValueSet WITH(NOLOCK) WHERE OrderLineId = @OrderLineId AND ContentTagId = @Id					
					UPDATE OrderEmailTemplateTagValueSet
					SET Value = @val
					WHERE OrderEmailTemplateTagValueId = @OrderEmailTemplateTagValueId AND ContentTagId = @Id
					SET @Val = '';
					
					----------------
					FETCH NEXT FROM @IdCursor INTO @Id;
					END

					-----------------------------------------


					update OrderLine
					set OrderEmailTemplateVersionId = @OrderEmailTemplateVersionId, EmailTemplateVersionId = null
					where id = @OrderLineId	
				
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;
				END TRY
				BEGIN CATCH
					SELECT ERROR_MESSAGE() AS [ErrorMessage];
					ROLLBACK TRAN
					insert into #RunningEmailResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, '', 'Fail', '', 'Email');
					select ProductCode, OrderNumber, TagName, Status, OperationType, [Type] from #RunningEmailResult;
					return
				END CATCH
				COMMIT TRAN	
			end

		
		end
  ---------------------------------------
END

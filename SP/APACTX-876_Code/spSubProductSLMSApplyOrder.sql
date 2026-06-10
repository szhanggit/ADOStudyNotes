IF EXISTS (
		SELECT NULL
		FROM dbo.sysobjects
		WHERE id = object_id(N'[dbo].[spSubProductSLMSApplyOrder]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
	DROP PROCEDURE [dbo].[spSubProductSLMSApplyOrder]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spSubProductSLMSApplyOrder] @ProductCode varchar(50)
	,@OrderNumber varchar(50), @RecordId INT, @SingleList SingleListType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Id INT;
	DECLARE @TagId int;
	DECLARE @TagName varchar(50);
	DECLARE @ProductSLMSTemplateTagValueId int;
	DECLARE @OrderSLMSTemplateTagValueId int;
	DECLARE @ProductSLMSTemplateVersionId int;
	DECLARE @OrderSLMSTemplateVersionId int;
	DECLARE @OrderSLMSTemplateVersionSetId int;
	DECLARE @OrderLineId int;
	DECLARE @SLMSTemplateVersionId int;
  	DECLARE @BusinessCursor as CURSOR;
	DECLARE @IdCursor AS CURSOR;
	DECLARE @val nvarchar(max);
	DECLARE @IsProductNewSLMSTemplate bit = 0;
	DECLARE @IsOrderNewSLMSTemplate bit = 0;
	
	create table dbo.#RunningSLMSResult
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
		where p.ProductCode = @ProductCode and o.OrderNumber = @OrderNumber and ol.SLMSTemplateVersionId is not null and ol.OrderSLMSTemplateVersionId is null)
		begin--Old Version
			set @IsOrderNewSLMSTemplate = 0;
		end
		else
		begin--New Version
			set @IsOrderNewSLMSTemplate = 1;
		end
  
  
    if exists(select * from ClientQuotationProduct cqp with(nolock) 
		inner join OrderLine ol with(nolock) on ol.ClientQuotationProductId = cqp.Id
		inner join [Order] o with(nolock) on o.Id = ol.OrderId
		inner join ProductVersion pv with(nolock) on cqp.ProductVersionId = pv.Id
		inner join Product p with(nolock) on pv.ProductId = p.Id
		where p.ProductCode = @ProductCode and o.OrderNumber = @OrderNumber and ol.SLMSTemplateVersionId is null and ol.OrderSLMSTemplateVersionId is null)
		begin--Old Version
			set @IsOrderNewSLMSTemplate = 0;
		end


	if exists(SELECT * FROM Product WITH(nolock) where ProductCode = @ProductCode and SLMSTemplateVersionId is not null and ProductSLMSTemplateVersionId is null)
		begin--Old Version
			set @IsProductNewSLMSTemplate = 0;
		end
		else
		begin--New Version
			set @IsProductNewSLMSTemplate = 1;
		end
  
  
  ---------------------------------------

  if @IsOrderNewSLMSTemplate = 0 and @IsProductNewSLMSTemplate = 0
		begin--Old Version
			select @OrderLineId = ol.Id, @SLMSTemplateVersionId = ol.SLMSTemplateVersionId 
			from OrderLine ol with(nolock) 
			inner join [Order] o with(nolock) on ol.OrderId = o.Id 
			inner join ClientQuotationProduct cqp with(nolock) on ol.ClientQuotationProductId = cqp.Id
			inner join ProductVersion pv on pv.Id = cqp.ProductVersionId
			inner join Product p on p.Id = pv.ProductId
			where p.ProductCode = @ProductCode and o.OrderNumber = @OrderNumber
			
			if(@OrderLineId is null or @OrderLineId = '')
			BEGIN
				return
			END		
		
			if(@SLMSTemplateVersionId is null or @SLMSTemplateVersionId = '')
			BEGIN
				return
			END		
		
			update OrderLine
			set SLMSTemplateVersionId = @SLMSTemplateVersionId
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


				select @val = [Value] from ProductSLMSContentTagValueSet pes with(nolock)
				where pes.ProductSLMSTemplateTagValueId = @ProductSLMSTemplateTagValueId
				and pes.ContentTagId = @TagId;


				IF @val is not null and @val <> ''  
				BEGIN		
					select @TagName = TagName from ContentTag where Id = @TagId;
					if exists(select * from OrderSLMSContentTagValueSet with(nolock) where OrderLineId = @OrderLineId and ContentTagId = @TagId)
						begin
							update OrderSLMSContentTagValueSet
							set [Value] = @val
							where OrderLineId = @OrderLineId and ContentTagId = @TagId
							
							insert into #RunningSLMSResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, @TagName, 'Success', 'Update', 'SLMS');
						end
					else
						begin
							insert into OrderSLMSContentTagValueSet(ContentTagId, OrderLineId, Value) 
							values (@TagId, @OrderLineId, @val);			
								
							insert into #RunningSLMSResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, @TagName, 'Success', 'Insert', 'SLMS');
						end
				END

				set @val = '';				
				----------------
				FETCH NEXT FROM @BusinessCursor INTO @TagId;
				END
				
				
				UPDATE ProductEmailSLMSTemplateApplyOrderLog
				SET Status = 1
				WHERE Id = @RecordId;

				select ProductCode, OrderNumber, TagName, Status, OperationType, [Type] from #RunningSLMSResult;	
			END TRY
			BEGIN CATCH
					ROLLBACK TRAN
					return
			END CATCH

			COMMIT TRAN					
		
		end
  else
		begin--New Version

		
			select @ProductSLMSTemplateTagValueId = SLMSTemplateTagValueId, @ProductSLMSTemplateVersionId = SLMSTemplateVersionId from ProductSLMSTemplateVersionSet with(nolock)
			where ProductSLMSTemplateVersionId in (select ProductSLMSTemplateVersionId from Product with(nolock) where ProductCode = @ProductCode)

			if(@ProductSLMSTemplateTagValueId is null or @ProductSLMSTemplateTagValueId = '')
			BEGIN
				UPDATE ProductEmailSLMSTemplateApplyOrderLog
				SET Status = 1
				WHERE Id = @RecordId;			
				return
			END

			if(@ProductSLMSTemplateVersionId is null or @ProductSLMSTemplateVersionId = '')
			BEGIN
				UPDATE ProductEmailSLMSTemplateApplyOrderLog
				SET Status = 1
				WHERE Id = @RecordId;			
				return
			END		
		
		
		
			if @IsOrderNewSLMSTemplate = 1
			begin 
				select @OrderSLMSTemplateVersionSetId = Id, @OrderSLMSTemplateTagValueId = SLMSTemplateTagValueId, @OrderSLMSTemplateVersionId = OrderSLMSTemplateVersionId, @OrderSLMSTemplateVersionId = SLMSTemplateVersionId from OrderSLMSTemplateVersionSet with(nolock) where OrderSLMSTemplateVersionId in (
					select ol.OrderSLMSTemplateVersionId from ClientQuotationProduct cqp with(nolock) 
					inner join OrderLine ol with(nolock) on ol.ClientQuotationProductId = cqp.Id
					inner join [Order] o with(nolock) on o.Id = ol.OrderId
					inner join ProductVersion pv with(nolock) on cqp.ProductVersionId = pv.Id
					inner join Product p with(nolock) on pv.ProductId = p.Id
					where p.ProductCode = @ProductCode and o.OrderNumber = @OrderNumber
				) 
				
				if(@OrderSLMSTemplateVersionSetId is null or @OrderSLMSTemplateVersionSetId = '')
				BEGIN
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;
					return
				END

				if(@OrderSLMSTemplateTagValueId is null or @OrderSLMSTemplateTagValueId = '')
				BEGIN
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;
					return
				END
				
				if(@OrderSLMSTemplateVersionId is null or @OrderSLMSTemplateVersionId = '')
				BEGIN
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;
					return
				END
				
				IF NOT EXISTS (SELECT * FROM OrderSLMSTemplateVersionSet WITH(NOLOCK) WHERE SLMSTemplateVersionId = @ProductSLMSTemplateVersionId AND OrderSLMSTemplateVersionId = @OrderSLMSTemplateVersionId)
				BEGIN
					BEGIN TRY
						update OrderSLMSTemplateVersionSet
						set SLMSTemplateVersionId = @ProductSLMSTemplateVersionId
						where id = @OrderSLMSTemplateVersionSetId		
					END TRY
					BEGIN CATCH
						UPDATE ProductEmailSLMSTemplateApplyOrderLog
						SET Status = 1
						WHERE Id = @RecordId;
					END CATCH;
				END

				BEGIN TRAN

				BEGIN TRY
					SET @BusinessCursor = CURSOR FOR
					select Id from ContentTag

					OPEN @BusinessCursor;
					FETCH NEXT FROM @BusinessCursor INTO @TagId;
					WHILE @@FETCH_STATUS = 0
					BEGIN
					-------------
					

					select @val = [Value] from ProductSLMSContentTagValueSet pes with(nolock)
					where pes.ProductSLMSTemplateTagValueId = @ProductSLMSTemplateTagValueId
					and pes.ContentTagId = @TagId;


					IF @val is not null and @val <> ''  
					BEGIN		
						select @TagName = TagName from ContentTag where Id = @TagId;
						if exists(select * from OrderSLMSTemplateTagValueSet with(nolock) where OrderSLMSTemplateTagValueId = @OrderSLMSTemplateTagValueId and ContentTagId = @TagId)
							BEGIN
								IF NOT EXISTS(SELECT * FROM @SingleList WHERE Id = @TagId)
								BEGIN
									update OrderSLMSTemplateTagValueSet
									set [Value] = @val
									where OrderSLMSTemplateTagValueId = @OrderSLMSTemplateTagValueId and ContentTagId = @TagId;
								
									insert into #RunningSLMSResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, @TagName, 'Success', 'Update', 'SLMS');
								END
							end
						else
							begin
								insert into  OrderSLMSTemplateTagValueSet(ContentTagId, OrderSLMSTemplateTagValueId, Value) 
								values (@TagId, @OrderSLMSTemplateTagValueId, @val);				
								
								insert into #RunningSLMSResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, @TagName, 'Success', 'Insert', 'SLMS');
							end
					END

					set @val = '';				
					----------------
					FETCH NEXT FROM @BusinessCursor INTO @TagId;
					END
					
					
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;

					select ProductCode, OrderNumber, TagName, Status, OperationType, [Type] from #RunningSLMSResult;	
				END TRY
				BEGIN CATCH
						SELECT ERROR_MESSAGE() AS [ErrorMessage];
						ROLLBACK TRAN
						insert into #RunningSLMSResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, '', 'Fail', '', 'SLMS');
						select ProductCode, OrderNumber, TagName, Status, OperationType, [Type] from #RunningSLMSResult;
						return
				END CATCH

				COMMIT TRAN			
			end
			else
			begin--It is old order template. It needs updating to the new one.
				BEGIN TRAN
				BEGIN TRY


					select @OrderSLMSTemplateTagValueId = Max(Id) from OrderSLMSTemplateTagValue with(nolock);
					Set @OrderSLMSTemplateTagValueId = @OrderSLMSTemplateTagValueId + 1;
					select @OrderSLMSTemplateVersionId = Max(Id) from OrderSLMSTemplateVersionSet with(nolock);
					Set @OrderSLMSTemplateVersionId = @OrderSLMSTemplateVersionId + 1;
			
					INSERT OrderSLMSTemplateTagValue DEFAULT VALUES;
					SELECT @OrderSLMSTemplateTagValueId = SCOPE_IDENTITY();
				
					INSERT OrderSLMSTemplateVersion DEFAULT VALUES;
					SELECT @OrderSLMSTemplateVersionId = SCOPE_IDENTITY();
				
					INSERT INTO OrderSLMSTemplateTagValueSet
					SELECT pes.ContentTagId as ContentTagId, @OrderSLMSTemplateTagValueId as OrderSLMSTemplateTagValueId, [Value] as [Value] FROM ProductSLMSContentTagValueSet pes WITH(NOLOCK) WHERE pes.ProductSLMSTemplateTagValueId = @ProductSLMSTemplateTagValueId;
				
					insert into OrderSLMSTemplateVersionSet (OrderSLMSTemplateVersionId, SLMSTemplateVersionId, SLMSTemplateTagValueId) values (@OrderSLMSTemplateVersionId, @ProductSLMSTemplateVersionId, @OrderSLMSTemplateTagValueId);
				
					SELECT @OrderLineId = ol.Id FROM [Order] o WITH(NOLOCK) 
					INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId 
					INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON ol.ClientQuotationProductId = cqp.Id
					INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON pv.Id = cqp.ProductVersionId
					INNER JOIN dbo.Product p WITH(NOLOCK) ON pv.ProductId = p.Id
					WHERE o.OrderNumber = @OrderNumber AND p.ProductCode = @ProductCode				
				
					if(@OrderLineId is null or @OrderLineId = '')
					BEGIN
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

					SELECT @val = Value FROM OrderSLMSContentTagValueSet WITH(NOLOCK) WHERE OrderLineId = @OrderLineId AND ContentTagId = @Id					
					UPDATE OrderSLMSTemplateTagValueSet
					SET Value = @val
					WHERE OrderSLMSTemplateTagValueId = @OrderSLMSTemplateTagValueId AND ContentTagId = @Id
					SET @Val = '';
					
					----------------
					FETCH NEXT FROM @IdCursor INTO @Id;
					END

					-----------------------------------------


					update OrderLine
					set OrderSLMSTemplateVersionId = @OrderSLMSTemplateVersionId, SLMSTemplateVersionId = null
					where id = @OrderLineId	
				
					UPDATE ProductEmailSLMSTemplateApplyOrderLog
					SET Status = 1
					WHERE Id = @RecordId;
				END TRY
				BEGIN CATCH
					SELECT ERROR_MESSAGE() AS [ErrorMessage];
					ROLLBACK TRAN
					insert into #RunningSLMSResult (ProductCode, OrderNumber, TagName, Status, OperationType, [Type]) values (@ProductCode, @OrderNumber, '', 'Fail', '', 'SLMS');
					select ProductCode, OrderNumber, TagName, Status, OperationType, [Type] from #RunningSLMSResult;
					return
				END CATCH
				COMMIT TRAN	
			end

		
		end
  ---------------------------------------
END

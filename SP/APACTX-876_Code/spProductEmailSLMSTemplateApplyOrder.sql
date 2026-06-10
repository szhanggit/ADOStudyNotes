IF EXISTS (
		SELECT NULL
		FROM dbo.sysobjects
		WHERE id = object_id(N'[dbo].[spProductEmailSLMSTemplateApplyOrder]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
	DROP PROCEDURE [dbo].[spProductEmailSLMSTemplateApplyOrder]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spProductEmailSLMSTemplateApplyOrder
@InputTable ProductEmailSLMSTemplateApplyOrderType READONLY,
@SingleList SingleListType READONLY
AS
      SET NOCOUNT ON
	  DECLARE @TableID int;
	  DECLARE @RecordId INT;
	  DECLARE @TaskId INT;
	  DECLARE @ProductCode varchar(50);
	  DECLARE @OrderNumber varchar(50);
	  DECLARE @IsEmail INT;
	  DECLARE @Status INT;
	  DECLARE @TaskStatus INT;
	  DECLARE @Counter int;
	  DECLARE @CheckCount INT;
	  DECLARE @BusinessCursor as CURSOR;

	  CREATE TABLE dbo.#InputVariableTable
	  ( 
	   Id INT PRIMARY KEY IDENTITY (1, 1), 
	   RecordId INT,
	   BatchLogId INT,
	   ProductCode VARCHAR(50),
       OrderNumber VARCHAR(50),
	   IsEmail INT
	  ) 

	  UPDATE dbo.ProductEmailSLMSTemplateApplyOrderTask
	  SET StartDate = GETDATE(), EndDate = null
	  WHERE Id IN (SELECT BatchLogId AS Id from @InputTable GROUP BY BatchLogId);
	  

	  insert into dbo.#InputVariableTable (RecordId, BatchLogId, ProductCode, OrderNumber, IsEmail)
	  SELECT RecordId, BatchLogId, ProductCode, OrderNumber, IsEmail from @InputTable;

	  while exists (select * from dbo.#InputVariableTable)
		begin

			select top 1 @TableID = id
			, @RecordId = RecordId
			, @TaskId = BatchLogId
			, @ProductCode = ProductCode
			, @OrderNumber = OrderNumber
			, @IsEmail = IsEmail
			from dbo.#InputVariableTable
			order by id asc;

			select @TaskStatus = TaskStatus from ProductEmailSLMSTemplateApplyOrderTask with(nolock) where Id = @TaskId;
			
			if @TaskStatus != 16
			BEGIN
				----------------
				SELECT @Status = [Status], @IsEmail = [IsEmail] FROM ProductEmailSLMSTemplateApplyOrderLog WITH(NOLOCK) WHERE Id = @RecordId;
				IF @Status = 0
				BEGIN
					IF @IsEmail = 1
					BEGIN--Email
						EXEC [dbo].[spSubProductEmailApplyOrder] @ProductCode, @OrderNumber, @RecordId, @SingleList;
					END
					ELSE
					BEGIN--SLMS
						EXEC [dbo].[spSubProductSLMSApplyOrder] @ProductCode, @OrderNumber, @RecordId, @SingleList;
					END
				END
				-----------------			
			END

			delete dbo.#InputVariableTable where id = @TableID;

		end		


		SET @BusinessCursor = CURSOR FOR
		SELECT BatchLogId AS Id from @InputTable GROUP BY BatchLogId

		OPEN @BusinessCursor;
		FETCH NEXT FROM @BusinessCursor INTO @TaskId;
		WHILE @@FETCH_STATUS = 0
		BEGIN
		-------------

		SELECT @CheckCount = COUNT(*)
		FROM dbo.ProductEmailSLMSTemplateApplyOrderTask bl WITH(NOLOCK) 
		INNER JOIN ProductEmailSLMSTemplateApplyOrderLog pes WITH(NOLOCK)
		ON bl.Id = pes.TaskId
		WHERE bl.Id = @TaskId AND pes.Status = 0;

		IF @CheckCount = 0
		BEGIN
			UPDATE dbo.ProductEmailSLMSTemplateApplyOrderTask
			SET [TaskStatus] = 8, EndDate = GETDATE()
			WHERE Id = @TaskId
		END

		set @CheckCount = 0;				
		----------------
		FETCH NEXT FROM @BusinessCursor INTO @TaskId;
		END


GO
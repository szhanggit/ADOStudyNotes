IF EXISTS (
		SELECT NULL
		FROM dbo.sysobjects
		WHERE id = object_id(N'[dbo].[spCheckApplyToOrderStatus]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
	DROP PROCEDURE [dbo].[spCheckApplyToOrderStatus]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spCheckApplyToOrderStatus
@ProductCode varchar(50),
@Status INT OUTPUT,
@ProcessNum INT OUTPUT,
@TotalNum INT OUTPUT
AS
    SET NOCOUNT ON
	DECLARE @TaskId INT;
	SELECT TOP 1 @Status = bl.TaskStatus, @TaskId = bl.Id FROM dbo.ProductEmailSLMSTemplateApplyOrderTask bl WITH(NOLOCK) 
	INNER JOIN ProductEmailSLMSTemplateApplyOrderLog pes WITH(NOLOCK)
	ON bl.Id = pes.TaskId
	WHERE pes.ProductCode = @ProductCode
	ORDER BY bl.CreateDate DESC
    
	IF @Status IS NULL OR @Status = ''
	BEGIN 
		SET @Status = -1;
	END

	SELECT @TotalNum = COUNT(*) FROM ProductEmailSLMSTemplateApplyOrderLog WITH(NOLOCK) WHERE TaskId = @TaskId;
	SELECT @ProcessNum = COUNT(*) FROM ProductEmailSLMSTemplateApplyOrderLog WITH(NOLOCK) 
	WHERE TaskId = @TaskId AND Status = 1;

GO


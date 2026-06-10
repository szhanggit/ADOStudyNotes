IF EXISTS (
		SELECT NULL
		FROM dbo.sysobjects
		WHERE id = object_id(N'[dbo].[spSaveAndApplyToOrder]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
	DROP PROCEDURE [dbo].[spSaveAndApplyToOrder]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spSaveAndApplyToOrder
@ProductCode varchar(50),
@OrderNumber INT OUTPUT,
@TaskId BIGINT OUTPUT
AS
    SET NOCOUNT ON
	DECLARE @ProductId INT;

	EXEC spMakeProductEmailTemplateToLatestVersion @ProductCode;
	EXEC spMakeProductSLMSTemplateToLatestVersion @ProductCode;

	SELECT @ProductId = Id FROM dbo.Product WITH(NOLOCK) WHERE ProductCode = @ProductCode;
	select @OrderNumber = COUNT(*) from [ORDER] o WITH(nolock)
	INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId
	INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON cqp.Id = ol.ClientQuotationProductId
	INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON cqp.ProductVersionId = pv.Id
	INNER JOIN dbo.Product p WITH(NOLOCK) ON p.Id = pv.ProductId
	where p.ProductCode = @ProductCode

	IF @OrderNumber > 0
	BEGIN
		INSERT INTO ProductEmailSLMSTemplateApplyOrderTask (CreateDate,StartDate, EndDate, [TaskStatus]) 
		VALUES (GETDATE(), NULL, NULL, 1);
		SELECT @TaskId = SCOPE_IDENTITY();

		INSERT INTO ProductEmailSLMSTemplateApplyOrderLog (TaskId, ProductCode, OrderNumber, IsEmail, [Status])
		select @TaskId AS TaskId, @ProductCode AS ProductCode, o.OrderNumber AS OrderNumber, 1 AS IsEmail, 0 AS [Status] from [ORDER] o WITH(nolock)
		INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId
		INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON cqp.Id = ol.ClientQuotationProductId
		INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON cqp.ProductVersionId = pv.Id
		INNER JOIN dbo.Product p WITH(NOLOCK) ON p.Id = pv.ProductId
		where p.ProductCode = @ProductCode

		INSERT INTO ProductEmailSLMSTemplateApplyOrderLog (TaskId, ProductCode, OrderNumber, IsEmail, [Status])
		select @TaskId AS TaskId, @ProductCode AS ProductCode, o.OrderNumber AS OrderNumber, 0 AS IsEmail, 0 AS [Status] from [ORDER] o WITH(nolock)
		INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId
		INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON cqp.Id = ol.ClientQuotationProductId
		INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON cqp.ProductVersionId = pv.Id
		INNER JOIN dbo.Product p WITH(NOLOCK) ON p.Id = pv.ProductId
		where p.ProductCode = @ProductCode
	END
GO




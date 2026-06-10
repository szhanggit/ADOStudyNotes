IF EXISTS (
		SELECT NULL
		FROM dbo.sysobjects
		WHERE id = object_id(N'[dbo].[spMakeProductSLMSTemplateToLatestVersion]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
	DROP PROCEDURE [dbo].[spMakeProductSLMSTemplateToLatestVersion]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spMakeProductSLMSTemplateToLatestVersion
@ProductCode varchar(50)
AS
    SET NOCOUNT ON
	DECLARE @ProductId INT;
	DECLARE @SLMSTemplateId INT;
	DECLARE @SLMSTemplateVersionId INT;
	DECLARE @ProductSLMSTemplateVersionId INT;
	DECLARE @ProductSLMSTemplateVersionSetId INT;
	DECLARE @P_SLMSTemplateVersionId INT;
	DECLARE @P_SLMSTemplateTagValueId INT;
	DECLARE @P_ProductSLMSTemplateVersionId INT;


	SELECT @ProductId = p.Id, @P_SLMSTemplateVersionId = p.SLMSTemplateVersionId, @P_ProductSLMSTemplateVersionId = p.ProductSLMSTemplateVersionId, @P_SLMSTemplateTagValueId = SLMSTemplateTagValueId 
	FROM dbo.Product p WITH(NOLOCK) 
	WHERE p.ProductCode = @ProductCode

	delete FROM dbo.SpecialSLMSTemplateVersion WHERE ReferId = 1 AND ReferId = @ProductId;

	if @P_SLMSTemplateVersionId is not null And @P_SLMSTemplateVersionId != ''
	begin--Old template
			SELECT @SLMSTemplateId = et.Id
			FROM dbo.Product p WITH(NOLOCK) 
			INNER JOIN dbo.SLMSTemplateVersion etv WITH(NOLOCK) ON p.SLMSTemplateVersionId = etv.Id
			INNER JOIN dbo.SLMSTemplate et WITH(NOLOCK) ON etv.TemplateId = et.Id
			WHERE p.ProductCode = @ProductCode
	
			SELECT @SLMSTemplateVersionId = Id FROM dbo.SLMSTemplateVersion WITH(NOLOCK) WHERE IsCurrentVersion = 1 and TemplateId = @SLMSTemplateId
			
			INSERT ProductSLMSTemplateVersion DEFAULT VALUES;
			SELECT @ProductSLMSTemplateVersionId = SCOPE_IDENTITY();
			
			insert into ProductSLMSTemplateVersionSet (ProductSLMSTemplateVersionId, SLMSTemplateVersionId, SLMSTemplateTagValueId) values (@ProductSLMSTemplateVersionId, @SLMSTemplateVersionId, @P_SLMSTemplateTagValueId);
			Update Product
			set SLMSTemplateVersionId = null, SLMSTemplateTagValueId = null, ProductSLMSTemplateVersionId = @ProductSLMSTemplateVersionId
			where ProductCode = @ProductCode;
	end
	else
	begin--New template
		if @P_ProductSLMSTemplateVersionId is not null And @P_ProductSLMSTemplateVersionId != ''
		begin
			SELECT @SLMSTemplateId = et.Id, @ProductSLMSTemplateVersionSetId = petvs.Id
			FROM dbo.Product p WITH(NOLOCK) 
			INNER JOIN ProductSLMSTemplateVersionSet petvs WITH(NOLOCK) ON p.ProductSLMSTemplateVersionId = petvs.ProductSLMSTemplateVersionId
			INNER JOIN dbo.SLMSTemplateVersion etv WITH(NOLOCK) ON petvs.SLMSTemplateVersionId = etv.Id
			INNER JOIN dbo.SLMSTemplate et WITH(NOLOCK) ON etv.TemplateId = et.Id
			WHERE p.ProductCode = @ProductCode
	
			SELECT @SLMSTemplateVersionId = Id FROM dbo.SLMSTemplateVersion WITH(NOLOCK) WHERE IsCurrentVersion = 1 and TemplateId = @SLMSTemplateId

			UPDATE ProductSLMSTemplateVersionSet
			SET SLMSTemplateVersionId = @SLMSTemplateVersionId
			WHERE Id = @ProductSLMSTemplateVersionSetId	
		end	
	end

GO	  
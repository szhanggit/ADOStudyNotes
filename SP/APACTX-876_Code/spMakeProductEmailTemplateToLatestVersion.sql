IF EXISTS (
		SELECT NULL
		FROM dbo.sysobjects
		WHERE id = object_id(N'[dbo].[spMakeProductEmailTemplateToLatestVersion]')
			AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
	DROP PROCEDURE [dbo].[spMakeProductEmailTemplateToLatestVersion]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spMakeProductEmailTemplateToLatestVersion
@ProductCode varchar(50)
AS
    SET NOCOUNT ON
	DECLARE @EmailTemplateId INT;
	DECLARE @EmailTemplateVersionId INT;
	DECLARE @ProductEmailTemplateVersionId Int;
	DECLARE @ProductEmailTemplateVersionSetId INT;
	DECLARE @P_EmailTemplateVersionId INT;
	DECLARE @P_EmailTemplateTagValueId INT;
	DECLARE @P_ProductEmailTemplateVersionId INT;

	SELECT @P_EmailTemplateVersionId = p.EmailTemplateVersionId, @P_ProductEmailTemplateVersionId = p.ProductEmailTemplateVersionId, @P_EmailTemplateTagValueId = EmailTemplateTagValueId 
	FROM dbo.Product p WITH(NOLOCK) 
	WHERE p.ProductCode = @ProductCode

	if @P_EmailTemplateVersionId is not null And @P_EmailTemplateVersionId != ''
	begin--Old template
			SELECT @EmailTemplateId = et.Id
			FROM dbo.Product p WITH(NOLOCK) 
			INNER JOIN dbo.EmailTemplateVersion etv WITH(NOLOCK) ON p.EmailTemplateVersionId = etv.Id
			INNER JOIN dbo.EmailTemplate et WITH(NOLOCK) ON etv.TemplateId = et.Id
			WHERE p.ProductCode = @ProductCode
	
			SELECT @EmailTemplateVersionId = Id FROM dbo.EmailTemplateVersion WITH(NOLOCK) WHERE IsCurrentVersion = 1 and TemplateId = @EmailTemplateId
			
			INSERT ProductEmailTemplateVersion DEFAULT VALUES;
			SELECT @ProductEmailTemplateVersionId = SCOPE_IDENTITY();
			
			insert into ProductEmailTemplateVersionSet (ProductEmailTemplateVersionId, EmailTemplateVersionId, EmailTemplateTagValueId) values (@ProductEmailTemplateVersionId, @EmailTemplateVersionId, @P_EmailTemplateTagValueId);
			Update Product
			set EmailTemplateVersionId = null, EmailTemplateTagValueId = null, ProductEmailTemplateVersionId = @ProductEmailTemplateVersionId
			where ProductCode = @ProductCode;
	end
	else
	begin--New template
		if @P_ProductEmailTemplateVersionId is not null And @P_ProductEmailTemplateVersionId != ''
		begin
			SELECT @EmailTemplateId = et.Id, @ProductEmailTemplateVersionSetId = petvs.Id
			FROM dbo.Product p WITH(NOLOCK) 
			INNER JOIN ProductEmailTemplateVersionSet petvs WITH(NOLOCK) ON p.ProductEmailTemplateVersionId = petvs.ProductEmailTemplateVersionId
			INNER JOIN dbo.EmailTemplateVersion etv WITH(NOLOCK) ON petvs.EmailTemplateVersionId = etv.Id
			INNER JOIN dbo.EmailTemplate et WITH(NOLOCK) ON etv.TemplateId = et.Id
			WHERE p.ProductCode = @ProductCode
	
			SELECT @EmailTemplateVersionId = Id FROM dbo.EmailTemplateVersion WITH(NOLOCK) WHERE IsCurrentVersion = 1 and TemplateId = @EmailTemplateId

			--SELECT @EmailTemplateVersionId AS EmailTemplateVersionId, @ProductEmailTemplateVersionSetId AS ProductEmailTemplateVersionSetId;

			UPDATE ProductEmailTemplateVersionSet
			SET EmailTemplateVersionId = @EmailTemplateVersionId
			WHERE Id = @ProductEmailTemplateVersionSetId	
		end	
	end
GO	  
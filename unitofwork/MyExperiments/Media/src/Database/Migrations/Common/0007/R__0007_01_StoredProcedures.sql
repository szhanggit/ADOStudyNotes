SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Alter sp_ins_media
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_ins_media];  
GO
CREATE PROCEDURE [media].[sp_ins_media]
	-- Add the parameters for the stored procedure here
	@FileName VARCHAR(100),
	@FileContentType VARCHAR(100),
	@Account VARCHAR(250),
	@BlobName VARCHAR(250),
	@Type Int,
	@Width VARCHAR(7),
	@Height VARCHAR(7),
	@Keyword NVARCHAR(250),
	@NodeUrl VARCHAR(250),
	@MediaId INT OUTPUT
AS
BEGIN

	SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;
        
		INSERT INTO media.tb_m_media 
		(
			[file_name],
			file_content_type,
			account,
			blob_name,
			[type],
			width,
			height,
			keyword,
			node_url
		)
		VALUES
		(
			@FileName,
			@FileContentType,
			@Account,
			@BlobName,
			@Type,
			@Width,
			@Height,
			@Keyword,
			@NodeUrl
		);

		SET @MediaId = SCOPE_IDENTITY()

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH 

       IF @@TRANCOUNT > 0
       BEGIN
          ROLLBACK TRANSACTION
       END;

	   THROW;
    END CATCH;

    SET NOCOUNT, XACT_ABORT OFF;
END
GO
-- =============================================
-- Alter sp_upd_media
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_upd_media];  
GO
CREATE PROCEDURE [media].[sp_upd_media]
	-- Add the parameters for the stored procedure here
	@MediaId INT,
	@FileName VARCHAR(100),
	@FileContentType VARCHAR(100),
	@BlobName VARCHAR(250),
	@Width VARCHAR(7),
	@Height VARCHAR(7),
	@Keyword NVARCHAR(250),
	@NodeUrl VARCHAR(250)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Update media.tb_m_media
	SET 
		[file_name] = @FileName,
		[file_content_type] = @FileContentType,
		blob_name = @BlobName,
		width = @Width,
		height = @Height,
		keyword = @Keyword,
		node_url = @NodeUrl
	WHERE media_id = @MediaId
END
GO
-- =============================================
-- Alter sp_upd_media_name
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_upd_media_name];  
GO
CREATE PROCEDURE [media].[sp_upd_media_name]
	-- Add the parameters for the stored procedure here
	@MediaId INT,
	@Keyword NVARCHAR(250)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Update media.tb_m_media
	SET 
		keyword = @Keyword
	WHERE media_id = @MediaId
END
GO

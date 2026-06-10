SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 11/10/2021
-- Description:	To delete media by id
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_del_media];  
GO
CREATE PROCEDURE [media].[sp_del_media]
	-- Add the parameters for the stored procedure here
	@MediaId INT
AS
BEGIN
	DELETE FROM media.tb_m_media WHERE media_id = @MediaId
END
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
	@Keyword VARCHAR(250),
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
	@Keyword VARCHAR(250),
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
-- Alter sp_sel_media
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_sel_media];  
GO
CREATE PROCEDURE [media].[sp_sel_media]
	-- Add the parameters for the stored procedure here
	@MediaId VARCHAR(250)
AS
BEGIN
	SElECT  
		media_id MediaId,
		[file_name] AS [FileName],
		keyword AS Keyword,
		height AS Height,
		width AS Width,
		blob_name AS BlobName,
		[type] AS [Type],
		node_url AS NodeUrl
	FROM media.tb_m_media WHERE media_id = @MediaId	
END
GO
-- =============================================
-- Alter sp_sel_media_by_blob_name
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_sel_media_by_blob_name];  
GO
CREATE PROCEDURE [media].[sp_sel_media_by_blob_name]
	-- Add the parameters for the stored procedure here
	@BlobName VARCHAR(250),
	@MediaId INT
AS
BEGIN
	SElECT  
		media_id MediaId,
		[file_name] AS [FileName],
		keyword AS Keyword,
        height AS Height,
		width AS Width,
		blob_name AS BlobName,
		[type] AS [Type],
		node_url AS NodeUrl
	FROM media.tb_m_media WHERE blob_name = @BlobName	AND media_id = @MediaId
END
GO
-- =============================================
-- Alter sp_sel_media_list
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_sel_media_list];  
GO
CREATE PROCEDURE [media].[sp_sel_media_list]
-- Add the parameters for the stored procedure here
@RowCount int = 20,
@PageOffset int = 0,
@TotalCount int = 0 OUTPUT,
@SearchKeyWord VARCHAR(100),
@Type INT
AS
BEGIN

SElECT
media_id MediaId,
[file_name] AS [FileName],
keyword AS Keyword,
height AS Height,
width AS Width,
blob_name AS BlobName,
[type] AS [Type],
node_url AS NodeUrl
FROM media.tb_m_media
WHERE [type] = @Type
AND keyword LIKE IIF(@SearchKeyWord IS NULL, keyword, '%'+@SearchKeyWord+'%' )
Order BY MediaId
OFFSET @PageOffset ROWS
FETCH NEXT @RowCount ROWS ONLY

SET @TotalCount = @@ROWCOUNT

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
	@Keyword VARCHAR(100)
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
-- =============================================
-- Alter sp_any_media_by_name_type
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_any_media_by_name_type];  
GO
CREATE PROCEDURE [media].[sp_any_media_by_name_type]
	-- Add the parameters for the stored procedure here
	@Keyword VARCHAR(250),
	@Type INT,
	@IsHave bit OUTPUT
AS
BEGIN
	IF EXISTS (	SELECT * FROM media.tb_m_media where keyword = @Keyword AND [type] = @Type)
		BEGIN
			SET @IsHave = 1
		END
	ELSE
		BEGIN
			SET @IsHave = 0
		END

END
GO
-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 11/11/2021
-- Description:	get media list for tx2 connector
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_sel_media_list_tx_connector]
GO
CREATE PROCEDURE [media].[sp_sel_media_list_tx_connector]
	-- Add the parameters for the stored procedure here
	@SearchKey VARCHAR(250),
	@MediaCategory INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SElECT
		media_id MediaId,
		[file_name] AS [FileName],
		keyword AS Keyword,
		height AS Height,
		width AS Width,
		blob_name AS BlobName,
		[type] AS MediaCategory,
		node_url AS [Url]
	FROM media.tb_m_media
	WHERE [type] = @MediaCategory
	AND keyword LIKE IIF(@SearchKey IS NULL, keyword, '%'+@SearchKey+'%' )

END
GO
-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 11/11/2021
-- Description:	get media for tx2 connector
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_sel_media_tx_connector]
GO
CREATE PROCEDURE [media].[sp_sel_media_tx_connector]
	-- Add the parameters for the stored procedure here
	@MediaId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SElECT  
		media_id MediaId,
		[file_name] AS [FileName],
		keyword AS Keyword,
		height AS Height,
		width AS Width,
		blob_name AS BlobName,
		[type] AS MediaCategory,
		node_url AS [Url]
	FROM media.tb_m_media WHERE media_id = @MediaId	
END
GO
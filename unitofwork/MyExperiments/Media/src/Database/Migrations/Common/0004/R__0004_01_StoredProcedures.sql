SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--==============================================
DROP PROCEDURE IF EXISTS [media].[sp_any_media_by_blob_name]
GO
--==============================================
-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 10/13/2021
-- Description:	insert media 
-- =============================================
IF NOT EXISTS (
        SELECT type_desc, type
        FROM sys.procedures WITH(NOLOCK)
        WHERE NAME = '[media].[sp_ins_media]'
            AND type = 'P'
      ) -- add initial checker for 'old
     
DROP PROCEDURE IF EXISTS [media].[sp_ins_media]
GO
CREATE PROCEDURE [media].[sp_ins_media]
	-- Add the parameters for the stored procedure here
	@FileName VARCHAR(100),
	@FileContentType VARCHAR(100),
	@ImageDimension VARCHAR(100),
	@NodeId VARCHAR(250),
	@NodeUrl VARCHAR(MAX),
	@Account VARCHAR(250),
	@BlobName VARCHAR(250),
	@Type Int
AS
BEGIN



	SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;
        
		INSERT INTO media.tb_m_media 
		(
			[file_name],
			file_content_type,
			-- image_dimension,
			-- node_id,
			node_url,
			account,
			blob_name,
			[type]
		)
		VALUES
		(
			@FileName,
			@FileContentType,
			-- @ImageDimension,
			-- @NodeId,
			@NodeUrl,
			@Account,
			@BlobName,
			@Type
		);

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
GO

-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 10/18/2021
-- Description:	select media by blob name
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_any_media_by_name_type]
GO
CREATE PROCEDURE [media].[sp_any_media_by_name_type]
	-- Add the parameters for the stored procedure here
	@FileName VARCHAR(250),
	@Type INT,
	@IsHave bit OUTPUT
AS
BEGIN
	IF EXISTS (	SELECT * FROM media.tb_m_media where [file_name] = @FileName AND [type] = @Type)
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
-- Create date: 10/19/2021
-- Description:	update media
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_upd_media]
GO
CREATE PROCEDURE [media].[sp_upd_media]
	-- Add the parameters for the stored procedure here
	@MediaId INT,
	@FileName VARCHAR(100),
	@FileContentType VARCHAR(100),
	@ImageDimension VARCHAR(100),
	@NodeId VARCHAR(250),
	@NodeUrl VARCHAR(MAX),
	@BlobName VARCHAR(250)
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
		-- image_dimension = @ImageDimension,
		-- node_id = @NodeId,
		node_url = @NodeUrl,
		blob_name = @BlobName
	WHERE media_id = @MediaId
END
GO
-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 10/19/2021
-- Description:	select media by blob name
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_sel_media_by_blob_name]
GO
CREATE PROCEDURE [media].[sp_sel_media_by_blob_name]
	-- Add the parameters for the stored procedure here
	@BlobName VARCHAR(250)
AS
BEGIN
	SElECT  
		media_id MediaId,
		[file_name] AS [FileName],
		-- image_dimension AS ImageDimension,
		node_url As NodeUrl,
		blob_name AS BlobName,
		[type] AS [Type]
	FROM media.tb_m_media WHERE blob_name = @BlobName	
END
GO
-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 10/19/2021
-- Description:	select media by blob name
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_sel_media_list]
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
		-- image_dimension AS ImageDimension,
		node_url As NodeUrl,
		blob_name AS BlobName,
		[type] AS [Type]
	FROM media.tb_m_media
	WHERE [type] = @Type
	AND [file_name] LIKE IIF(@SearchKeyWord IS NULL, [file_name], '%'+@SearchKeyWord+'%' )
	Order BY MediaId
	OFFSET @PageOffset ROWS
	FETCH NEXT @RowCount ROWS ONLY

    SET @TotalCount = @@ROWCOUNT
END
GO
-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 10/20/2021
-- Description:	select media by media Id
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_sel_media]
GO
CREATE PROCEDURE [media].[sp_sel_media]
	-- Add the parameters for the stored procedure here
	@MediaId VARCHAR(250)
AS
BEGIN
	SElECT  
		media_id MediaId,
		[file_name] AS [FileName],
		-- image_dimension AS ImageDimension,
		node_url As NodeUrl,
		blob_name AS BlobName,
		[type] AS [Type]
	FROM media.tb_m_media WHERE media_id = @MediaId	
END
GO
-- =============================================
-- Author:		Alvin Bernardo
-- Create date: 10/20/2021
-- Description:	update media name
-- =============================================
DROP PROCEDURE IF EXISTS [media].[sp_upd_media_name]
GO
CREATE PROCEDURE [media].[sp_upd_media_name]
	-- Add the parameters for the stored procedure here
	@MediaId INT,
	@FileName VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Update media.tb_m_media
	SET 
		[file_name] = @FileName
	WHERE media_id = @MediaId
END
GO


drop procedure [media].[sp_sel_media_list_tx_connector]
GO


CREATE PROCEDURE [media].[sp_sel_media_list_tx_connector]
	-- Add the parameters for the stored procedure here
	@SearchKey NVARCHAR(250),
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
	AND keyword LIKE IIF(@SearchKey IS NULL, keyword, N'%'+@SearchKey+'%' )

END
GO


drop procedure [media].[sp_sel_media_list]
GO


CREATE PROCEDURE [media].[sp_sel_media_list]
-- Add the parameters for the stored procedure here
@RowCount int = 20,
@PageOffset int = 0,
@TotalCount int = 0 OUTPUT,
@SearchKeyWord NVARCHAR(100),
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
AND keyword LIKE IIF(@SearchKeyWord IS NULL, keyword, N'%'+@SearchKeyWord+'%' )
Order BY MediaId
OFFSET @PageOffset ROWS
FETCH NEXT @RowCount ROWS ONLY

SET @TotalCount = @@ROWCOUNT

END
GO



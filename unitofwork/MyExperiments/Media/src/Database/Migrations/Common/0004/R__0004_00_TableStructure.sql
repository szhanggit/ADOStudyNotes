IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'media' )
    EXEC('CREATE SCHEMA [media]');
GO

/*
=================================================================================
create new table tb_m_media
=================================================================================
*/

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'media' 
                 AND  TABLE_NAME = 'tb_m_media'))
BEGIN
   
    PRINT 'tb_m_media table creating'
    
CREATE TABLE [media].[tb_m_media](
	[media_id] [int] IDENTITY(1,1) NOT NULL,
	[file_name] [varchar](100) NOT NULL,
	[file_content_type] [varchar](50) NOT NULL,
	[image_dimension] [varchar](50) NULL,
	[node_id] [varchar](250) NULL,
	[node_url] [varchar](max) NULL,
    [account] [varchar](250) NULL,
    [blob_name] [varchar](250) NULL,
	[type] [int] NOT NULL
    PRIMARY KEY ([media_id] ASC)
);   
END
GO
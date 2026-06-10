IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'media' 
                 AND  TABLE_NAME = 'tb_m_media'))
BEGIN
    IF COL_LENGTH('media.tb_m_media', 'file_content_type') IS NOT NULL
		BEGIN
			ALTER TABLE media.tb_m_media ALTER COLUMN file_content_type varchar(255) null;
		END;
END
GO
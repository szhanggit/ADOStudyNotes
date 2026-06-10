IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'media' 
                 AND  TABLE_NAME = 'tb_m_media'))
BEGIN
	IF COL_LENGTH('media.tb_m_media', 'keyword') IS NOT NULL
		BEGIN
			ALTER TABLE media.tb_m_media ALTER COLUMN keyword nvarchar(250) null;
		END;
END
GO
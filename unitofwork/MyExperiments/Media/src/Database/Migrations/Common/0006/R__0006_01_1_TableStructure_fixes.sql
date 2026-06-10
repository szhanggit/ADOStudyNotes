/*********************************/
--Alter media table 
--11/10/2021
/*********************************/
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'media' 
                 AND  TABLE_NAME = 'tb_m_media'))
BEGIN

	IF COL_LENGTH('media.tb_m_media', 'image_dimension') IS NOT NULL
		BEGIN
			ALTER TABLE media.tb_m_media DROP COLUMN image_dimension;
		END;

	IF COL_LENGTH('media.tb_m_media', 'node_id') IS NOT NULL
		BEGIN
			ALTER TABLE media.tb_m_media DROP COLUMN node_id;
		END;

	IF COL_LENGTH('media.tb_m_media', 'width') IS NULL
		BEGIN
			ALTER TABLE media.tb_m_media ADD width varchar(7) NULL;
		END;

	IF COL_LENGTH('media.tb_m_media', 'height') IS NULL
		BEGIN
			ALTER TABLE media.tb_m_media ADD height varchar(7) NULL;
		END;
     
	IF COL_LENGTH('media.tb_m_media', 'keyword') IS NULL
		BEGIN
			ALTER TABLE media.tb_m_media ADD keyword varchar(250) NULL;
		END;
END
GO
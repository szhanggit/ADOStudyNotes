IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE OBJECT_NAME(object_id) = 'tb_m_media' AND name = 'timestamp')
BEGIN
	ALTER TABLE media.tb_m_media
	ADD timestamp 
END
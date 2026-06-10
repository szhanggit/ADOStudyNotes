IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id = OBJECT_ID('[client].[tb_cbi_client_basic_information]') AND name='description')
BEGIN	
	ALTER TABLE client.tb_cbi_client_basic_information
	ADD [description] nvarchar(500) null;
END
GO

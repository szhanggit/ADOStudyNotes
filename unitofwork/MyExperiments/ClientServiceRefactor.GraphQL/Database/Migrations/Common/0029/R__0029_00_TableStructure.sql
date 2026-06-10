IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE OBJECT_NAME(object_id) = 'tb_d_dictionary' AND name = 'timestamp')
BEGIN
	ALTER TABLE general.tb_d_dictionary
	ADD timestamp 
END

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE OBJECT_NAME(object_id) = 'tb_d_dictionarytranslation' AND name = 'timestamp')
BEGIN
	ALTER TABLE general.tb_d_dictionarytranslation
	ADD timestamp 
END

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE OBJECT_NAME(object_id) = 'tb_a_address' AND name = 'timestamp')
BEGIN
	ALTER TABLE general.tb_a_address
	ADD timestamp 
END

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE OBJECT_NAME(object_id) = 'tb_cbi_client_basic_information' AND name = 'timestamp')
BEGIN
	ALTER TABLE client.tb_cbi_client_basic_information
	ADD timestamp 
END
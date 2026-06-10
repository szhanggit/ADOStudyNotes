IF EXISTS (SELECT 1 FROM syscolumns WHERE id = OBJECT_ID('[client].[tb_cbi_client_basic_information]') AND name='crm_reference')
	alter table [client].[tb_cbi_client_basic_information] drop COLUMN crm_reference;
GO

IF EXISTS (SELECT 1 FROM syscolumns WHERE id = OBJECT_ID('[client].[tb_cbi_client_basic_information]') AND name='finance_reference')
	alter table [client].[tb_cbi_client_basic_information] drop COLUMN finance_reference ;
GO
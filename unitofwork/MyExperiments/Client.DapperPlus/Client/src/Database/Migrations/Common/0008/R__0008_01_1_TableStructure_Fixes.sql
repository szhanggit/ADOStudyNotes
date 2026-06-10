IF COL_LENGTH('[client].[tb_cbi_client_basic_information]', 'CRMReference') IS NOT NULL
BEGIN
	ALTER TABLE [client].[tb_cbi_client_basic_information] DROP COLUMN CRMReference	
END
GO

IF COL_LENGTH('[client].[tb_cbi_client_basic_information]', 'FinanceReference') IS NOT NULL
BEGIN
	ALTER TABLE [client].[tb_cbi_client_basic_information] DROP COLUMN FinanceReference	
END
GO
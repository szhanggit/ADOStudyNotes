IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'UK_tb_cbi_client_basic_information_identity_code') 
   ALTER TABLE [client].[tb_cbi_client_basic_information] ADD  CONSTRAINT [UK_tb_cbi_client_basic_information_identity_code] UNIQUE NONCLUSTERED 
(
	[identity_code] ASC
)
GO

IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'UK_tb_cbi_client_basic_information_client_name') 
   ALTER TABLE [client].[tb_cbi_client_basic_information] ADD  CONSTRAINT [UK_tb_cbi_client_basic_information_client_name] UNIQUE NONCLUSTERED 
(
	[client_name] ASC
)
GO

IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'IDX_tb_cbi_client_basic_information_invoice_register_number') 
   CREATE NONCLUSTERED INDEX [IDX_tb_cbi_client_basic_information_invoice_register_number] ON [client].[tb_cbi_client_basic_information]
(
	[invoice_register_number] ASC
)
GO

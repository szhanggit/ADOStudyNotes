IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[client].seq_client_identity_code') AND type = N'SO')
BEGIN
   
    PRINT 'seq_client_identity_code SEQUENCE creating'
    
	CREATE SEQUENCE [client].[seq_client_identity_code]
	 AS [bigint]
	 START WITH 1
	 INCREMENT BY 1
	 MINVALUE 1
	 MAXVALUE 9999999999
	 CYCLE 
	 CACHE    
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[client].tb_cbi_client_basic_information') 
    AND type = N'U')
BEGIN
   
    PRINT 'tb_cbi_client_basic_information table creating'
    
	CREATE TABLE [client].[tb_cbi_client_basic_information](
		[client_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[client_name] [nvarchar](100) NOT NULL,
		[identity_code] [varchar](50) NOT NULL,
		[voucher_issuer_id] [int] NULL,
		[invoice_register_number] [nvarchar](100) NOT NULL,
		[crm_reference] [nvarchar](100) NULL,
		[finance_reference] [nvarchar](100) NULL,
		[business_type_id] [int] NULL,
		[status] [tinyint] NOT NULL,
		[security_algorithm] [tinyint] NOT NULL,
		[security_key] [varchar](32) NOT NULL,
		[need_notification] [bit] NOT NULL,
		[notification_provider_code_id] [int] NULL,
		[logo_media_id] [int] NULL,
		[banner_media_id] [int] NULL,
		[email_header_media_id] [int] NULL,
		[email_footer_media_id] [int] NULL,
		[can_issue] [bit] NOT NULL,
		[mandatory_auto_billing] [bit] NULL,
		[invoice_title] [nvarchar](100) NULL,
		[sub_url] [nvarchar](6) NULL,
		[email_provider_code] [nvarchar](8) NULL,
		[email_sender_name] [nvarchar](255) NULL,
		[email_sender_address] [nvarchar](255) NULL,
		[apply_email_subject] [bit] NULL,
		[sms_provider_code] [nvarchar](8) NULL,
		[sms_sender_name] [nvarchar](255) NULL,
		[sms_entity_id] [nvarchar](30) NULL,
		[sales_email] [nvarchar](255) NULL,
		[contact_name] [nvarchar](30) NULL,
		[contact_email] [nvarchar](255) NULL,
		[contact_phone] [nvarchar](50) NULL,
		[Memo] [nvarchar](2000) NULL,
	 CONSTRAINT [PK_tb_cbi_client_basic_information_client_id] PRIMARY KEY CLUSTERED 
	(
		[client_id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_tb_cbi_client_basic_information_identity_code] UNIQUE NONCLUSTERED 
	(
		[identity_code] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_tb_cbi_client_basic_information_client_name] UNIQUE NONCLUSTERED 
	(
		[client_name] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [client].[tb_cbi_client_basic_information] ADD  DEFAULT ((0)) FOR [need_notification]

	ALTER TABLE [client].[tb_cbi_client_basic_information] ADD  DEFAULT ((1)) FOR [can_issue]
  
END
GO


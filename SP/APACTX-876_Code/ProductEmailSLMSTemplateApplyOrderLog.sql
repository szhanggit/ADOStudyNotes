SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductEmailSLMSTemplateApplyOrderLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductEmailSLMSTemplateApplyOrderLog](
	Id [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	TaskId [bigint] FOREIGN KEY REFERENCES ProductEmailSLMSTemplateApplyOrderTask(Id) NOT NULL,
	ProductCode varchar(50),
	OrderNumber varchar(50),
	IsEmail int,
	Status int,
 CONSTRAINT [PK_ProductEmailSLMSTemplateApplyOrderLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
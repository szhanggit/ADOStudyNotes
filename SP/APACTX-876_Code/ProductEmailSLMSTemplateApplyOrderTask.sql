SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductEmailSLMSTemplateApplyOrderTask]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductEmailSLMSTemplateApplyOrderTask](
	Id [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	CreateDate 	DateTime NOT NULL,
	StartDate 	DateTime,
	EndDate		DateTime, 
	TaskStatus	int
 CONSTRAINT [PK_ProductEmailSLMSTemplateApplyOrderTask] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
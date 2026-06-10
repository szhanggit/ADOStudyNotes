SELECT * FROM dbo.BatchLog WITH(NOLOCK) WHERE BatchName = 'ProductEmailSLMSTemplateApplyOrderJob' ORDER BY Id DESC


SELECT * FROM dbo.BatchLog WHERE BatchName = 'ProductEmailSLMSTemplateApplyOrderJob'
SELECT * FROM ProductEmailSLMSTemplateApplyOrderLog WHERE TaskId = 2
SELECT * FROM ProductEmailSLMSTemplateApplyOrderTask
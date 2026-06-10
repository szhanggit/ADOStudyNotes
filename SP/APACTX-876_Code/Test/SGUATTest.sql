SELECT Id, EmailTemplateVersionId, ProductEmailTemplateVersionId FROM dbo.Product WITH(NOLOCK) WHERE ProductCode = 'GONPDDMMYY01BDTEST'
SELECT ol.Id, ol.EmailTemplateVersionId, ol.OrderEmailTemplateVersionId FROM [Order] o WITH(NOLOCK) 
INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId 
INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON ol.ClientQuotationProductId = cqp.Id
INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON pv.Id = cqp.ProductVersionId
INNER JOIN dbo.Product p WITH(NOLOCK) ON pv.ProductId = p.Id
WHERE o.OrderNumber = '201910041252' AND p.ProductCode = 'GONPDDMMYY01BDTEST'


	select * from ProductEmailTemplateVersionSet where ProductEmailTemplateVersionId = 1708
	select * from OrderEmailTemplateVersionSet where OrderEmailTemplateVersionId = 792

select o.OrderNumber, v.VoucherNumber, v.BeneficiaryInfoId, c.IdentityCode from Voucher v WITH(nolock)
join OrderBeneficiaryInfo obi WITH(nolock) on obi.Id = v.BeneficiaryInfoId 
join OrderLine ol WITH(nolock) on ol.Id = obi.OrderLineId 
join [Order] o WITH(NOLOCK) ON o.Id = ol.OrderId
JOIN dbo.ClientQuotationProduct cqp WITH(nolock) ON ol.ClientQuotationProductId = cqp.Id
JOIN dbo.ClientQuotation cq WITH(NOLOCK) ON cqp.ClientQuotationId = cq.Id
JOIN dbo.Client c WITH(NOLOCK) ON cq.ClientId = c.Id
JOIN dbo.ProductVersion pv WITH(NOLOCK) ON cqp.ProductVersionId = pv.Id
JOIN dbo.Product p WITH(NOLOCK) ON p.Id = pv.ProductId
where p.ProductCode = 'Product_test_003'



SELECT Id, EmailTemplateVersionId, ProductEmailTemplateVersionId FROM dbo.Product WITH(NOLOCK) WHERE ProductCode = 'Product_test_003'
SELECT ol.Id, ol.EmailTemplateVersionId, ol.OrderEmailTemplateVersionId FROM [Order] o WITH(NOLOCK) 
INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId 
INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON ol.ClientQuotationProductId = cqp.Id
INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON pv.Id = cqp.ProductVersionId
INNER JOIN dbo.Product p WITH(NOLOCK) ON pv.ProductId = p.Id
WHERE o.OrderNumber = '201905230926' AND p.ProductCode = 'Product_test_003'

	select * from ProductEmailTemplateVersionSet where ProductEmailTemplateVersionId = 1578
	select * from OrderEmailTemplateVersionSet where OrderEmailTemplateVersionId = 280


	SELECT * FROM dbo.EmailTemplateVersion WITH(NOLOCK) WHERE Id = 37
	SELECT * FROM dbo.EmailTemplateVersion WITH(NOLOCK) WHERE TemplateId = 5


SELECT * FROM dbo.BatchLog WHERE BatchName = 'ProductEmailSLMSTemplateApplyOrderJob'
SELECT * FROM ProductEmailSLMSTemplateApplyOrderLog
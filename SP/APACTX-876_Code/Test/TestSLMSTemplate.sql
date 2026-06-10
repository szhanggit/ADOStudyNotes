DECLARE @SingleList AS SingleListType;
/* Add data to the table variable. */
INSERT INTO @SingleList(Id) values (110);

EXEC [dbo].[spSubProductSLMSApplyOrder] 'MC0001', '201409170014', 17, @SingleList;
--------------------------------------------------------------------------------------------------------------------


SELECT * FROM dbo.BatchLog WHERE BatchName = 'ProductEmailSLMSTemplateApplyOrderJob'
SELECT * FROM ProductEmailSLMSTemplateApplyOrderLog
UPDATE ProductEmailSLMSTemplateApplyOrderLog
SET [Status] = 0
WHERE ProductCode = 'MC0001'
UPDATE dbo.BatchLog
SET [Status] = 1, ExecuteEndTime = GETDATE()
WHERE BatchName = 'ProductEmailSLMSTemplateApplyOrderJob'






SELECT Id, SLMSTemplateVersionId, ProductSLMSTemplateVersionId FROM dbo.Product WITH(NOLOCK) WHERE ProductCode = 'MC0001'
SELECT ol.Id, ol.SLMSTemplateVersionId, ol.OrderSLMSTemplateVersionId FROM [Order] o WITH(NOLOCK) 
INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId 
INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON ol.ClientQuotationProductId = cqp.Id
INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON pv.Id = cqp.ProductVersionId
INNER JOIN dbo.Product p WITH(NOLOCK) ON pv.ProductId = p.Id
WHERE o.OrderNumber = '201409170014' AND p.ProductCode = 'MC0001'

SELECT * FROM OrderSLMSContentTagValueSet WHERE OrderLineId = 10

UPDATE OrderLine
SET SLMSTemplateVersionId = null, OrderSLMSTemplateVersionId = NULL
WHERE Id = 10

select * from ProductSLMSTemplateVersionSet where ProductSLMSTemplateVersionId = 5273
select * from OrderSLMSTemplateVersionSet where OrderSLMSTemplateVersionId = 3075

select p.tn as TagName, p.Value as PValue, o.Value as OValue from 
(select ptv.SLMSTemplateTagValueId as id, pes.Value, pct.TagName as tn from ProductSLMSTemplateVersionSet ptv with(nolock) 
inner join ProductSLMSContentTagValueSet pes with(nolock) on ptv.SLMSTemplateTagValueId = pes.ProductSLMSTemplateTagValueId
inner join ContentTag pct with(nolock) on pct.Id = pes.ContentTagId
where ptv.Id = 5307) P
inner join
(select otv.SLMSTemplateTagValueId as id, oes.Value, pct.TagName as tn from OrderSLMSTemplateVersionSet otv with(nolock) 
inner join OrderSLMSTemplateTagValueSet oes with(nolock) on otv.SLMSTemplateTagValueId = oes.OrderSLMSTemplateTagValueId
inner join ContentTag pct with(nolock) on pct.Id = oes.ContentTagId
where otv.Id = 3091) o on p.tn = o.tn 




select oes.Id as id, oes.Value, pct.TagName as tn from OrderSLMSTemplateVersionSet otv with(nolock) 
	inner join OrderSLMSTemplateTagValueSet oes with(nolock) on otv.SLMSTemplateTagValueId = oes.OrderSLMSTemplateTagValueId
	inner join ContentTag pct with(nolock) on pct.Id = oes.ContentTagId
	where otv.Id = 6159



	SELECT * FROM OrderSLMSTemplateTagValueSet WHERE Id IN (6387)
	UPDATE OrderSLMSTemplateTagValueSet
	SET [Value] = '!!! asdf'
	WHERE Id IN (6387)
	SELECT * FROM ContentTag WHERE TagName = '{VOUCHER_NUMBER}'
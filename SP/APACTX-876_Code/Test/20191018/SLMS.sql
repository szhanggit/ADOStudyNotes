SELECT Id, SLMSTemplateVersionId, ProductSLMSTemplateVersionId FROM dbo.Product WITH(NOLOCK) WHERE ProductCode = 'AVSPB01'
SELECT ol.Id, ol.SLMSTemplateVersionId, ol.OrderSLMSTemplateVersionId FROM [Order] o WITH(NOLOCK) 
INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId 
INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON ol.ClientQuotationProductId = cqp.Id
INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON pv.Id = cqp.ProductVersionId
INNER JOIN dbo.Product p WITH(NOLOCK) ON pv.ProductId = p.Id
WHERE o.OrderNumber = '201910152049' AND p.ProductCode = 'AVSPB01'


	select * from ProductSLMSTemplateVersionSet where ProductSLMSTemplateVersionId = 3977
	select * from OrderSLMSTemplateVersionSet where OrderSLMSTemplateVersionId = 4930





	select p.tn as TagName, p.Value as PValue, o.Value as OValue from 
	(select ptv.SLMSTemplateTagValueId as id, pes.Value, pct.TagName as tn from ProductSLMSTemplateVersionSet ptv with(nolock) 
	inner join ProductSLMSContentTagValueSet pes with(nolock) on ptv.SLMSTemplateTagValueId = pes.ProductSLMSTemplateTagValueId
	inner join ContentTag pct with(nolock) on pct.Id = pes.ContentTagId
	where ptv.Id = 4007) P
	inner join
	(select otv.SLMSTemplateTagValueId as id, oes.Value, pct.TagName as tn from OrderSLMSTemplateVersionSet otv with(nolock) 
	inner join OrderSLMSTemplateTagValueSet oes with(nolock) on otv.SLMSTemplateTagValueId = oes.OrderSLMSTemplateTagValueId
	inner join ContentTag pct with(nolock) on pct.Id = oes.ContentTagId
	where otv.Id = 4954) o on p.tn = o.tn 
	where p.Value is not null and p.Value <> ''


UPDATE OrderSLMSTemplateTagValueSet
SET [Value] = 'Steven'
WHERE Id = 8919


SELECT * FROM ProductEmailSLMSTemplateApplyOrderLog WHERE ProductCode = 'AVSPB01' AND OrderNumber = '201910152049' AND Status = 0
SELECT * FROM ProductEmailSLMSTemplateApplyOrderTask


DECLARE @SingleList AS SingleListType;
/* Add data to the table variable. */
INSERT INTO @SingleList(Id) values (13);
INSERT INTO @SingleList(Id) values (23);

EXEC [dbo].[spSubProductSLMSApplyOrder] 'AVSPB01', '201910152049', 12, @SingleList;


SELECT * FROM dbo.ContentTag WITH(NOLOCK) WHERE TagName LIKE '%Greeting%'
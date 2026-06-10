SELECT * FROM dbo.ContentTag WITH(NOLOCK)
SELECT * FROM OrderLine WITH(NOLOCK) WHERE Id = 14004
SELECT * FROM [Order] o WITH(NOLOCK) WHERE o.Id = 8185
SELECT * FROM dbo.Product WITH(NOLOCK) WHERE ProductCode = 'AVSPB01'




SELECT Id, EmailTemplateVersionId, ProductEmailTemplateVersionId FROM dbo.Product WITH(NOLOCK) WHERE ProductCode = 'AVSPB01'
SELECT ol.Id, ol.EmailTemplateVersionId, ol.OrderEmailTemplateVersionId FROM [Order] o WITH(NOLOCK) 
INNER JOIN OrderLine ol WITH(NOLOCK) ON o.Id = ol.OrderId 
INNER JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON ol.ClientQuotationProductId = cqp.Id
INNER JOIN dbo.ProductVersion pv WITH(NOLOCK) ON pv.Id = cqp.ProductVersionId
INNER JOIN dbo.Product p WITH(NOLOCK) ON pv.ProductId = p.Id
WHERE o.OrderNumber = '201910152049' AND p.ProductCode = 'AVSPB01'


	select * from ProductEmailTemplateVersionSet where ProductEmailTemplateVersionId = 4776
	select * from OrderEmailTemplateVersionSet where OrderEmailTemplateVersionId = 5126





	select p.tn as TagName, p.Value as PValue, o.Value as OValue from 
	(select ptv.EmailTemplateTagValueId as id, pes.TextValue, pes.Value, pct.TagName as tn from ProductEmailTemplateVersionSet ptv with(nolock) 
	inner join ProductEmailContentTagValueSet pes with(nolock) on ptv.EmailTemplateTagValueId = pes.ProductEmailTemplateTagValueId
	inner join ContentTag pct with(nolock) on pct.Id = pes.ContentTagId
	where ptv.Id = 4786) P
	inner join
	(select otv.EmailTemplateTagValueId as id, oes.Value, pct.TagName as tn from OrderEmailTemplateVersionSet otv with(nolock) 
	inner join OrderEmailTemplateTagValueSet oes with(nolock) on otv.EmailTemplateTagValueId = oes.OrderEmailTemplateTagValueId
	inner join ContentTag pct with(nolock) on pct.Id = oes.ContentTagId
	where otv.Id = 5144) o on p.tn = o.tn 
	where p.Value is not null and p.Value <> ''
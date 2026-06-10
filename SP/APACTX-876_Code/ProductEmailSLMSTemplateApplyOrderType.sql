IF type_id('dbo.ProductEmailSLMSTemplateApplyOrderType') IS NOT NULL
        DROP TYPE dbo.ProductEmailSLMSTemplateApplyOrderType;
/* Create a User-Defined Table Type. */
CREATE TYPE ProductEmailSLMSTemplateApplyOrderType	
   AS TABLE
      (Id INT PRIMARY KEY, 
	    RecordId INT
	  ,	BatchLogId int
	  , ProductCode VARCHAR(50)
      , OrderNumber VARCHAR(50)
	  , IsEmail INT);
GO
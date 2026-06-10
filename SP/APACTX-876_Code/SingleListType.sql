IF type_id('dbo.SingleListType') IS NOT NULL
        DROP TYPE dbo.SingleListType;
/* Create a User-Defined Table Type. */
CREATE TYPE SingleListType	
   AS TABLE (Id INT);
GO
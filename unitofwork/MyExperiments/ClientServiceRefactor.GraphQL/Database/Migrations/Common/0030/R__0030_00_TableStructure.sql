IF EXISTS (SELECT 1 FROM sys.columns C INNER JOIN sys.tables T ON C.object_id = T.object_id
INNER JOIN sys.schemas S ON T.schema_id = S.schema_id
WHERE S.name = 'general' AND T.name = 'tb_d_dictionary' AND C.name = 'name')
BEGIN

	IF (SELECT C.max_length
	FROM sys.schemas S
	INNER JOIN sys.tables T ON S.schema_id = T.schema_id
	INNER JOIN sys.columns C ON T.object_id = C.object_id
	INNER JOIN sys.types TP ON C.user_type_id = TP.user_type_id
	WHERE S.name = 'general' AND T.name = 'tb_d_dictionary' AND C.name = 'name') <> 200
	BEGIN
		ALTER TABLE [general].[tb_d_dictionary]
		ALTER COLUMN [name] NVARCHAR(100) NOT NULL
	END
END
IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'general' )
    EXEC('CREATE SCHEMA [general]');
GO

IF not exists (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[general].tb_a_address')
    AND type = N'U')
begin
CREATE TABLE [general].[tb_a_address](
	[address_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[detail_address_line] [nvarchar](400) NULL,
	[district] [nvarchar](100) NULL,
	[city_id] [int] NULL,
	[state_province_id] [int] NULL,
	[postcode] [nvarchar](20) NULL,
	[country_id] [int] NULL,
	[longitude] [float] NULL,
	[latitude] [float] NULL,
	[status] [tinyint] NOT NULL,
	[tx2id] [int] NULL,
 CONSTRAINT [pk_tb_a_address] PRIMARY KEY CLUSTERED 
(
	[address_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO




if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Id')
begin
EXEC sp_RENAME '[general].[tb_a_address].Id' , 'address_id', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'DetailAddressLine')
begin
EXEC sp_RENAME '[general].[tb_a_address].DetailAddressLine' , 'detail_address_line', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'District')
begin
EXEC sp_RENAME '[general].[tb_a_address].District' , 'district', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'CityId')
begin
EXEC sp_RENAME '[general].[tb_a_address].CityId' , 'city_id', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'StateOrProvinceId')
begin
EXEC sp_RENAME '[general].[tb_a_address].StateOrProvinceId' , 'state_province_id', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Postcode')
begin
EXEC sp_RENAME '[general].[tb_a_address].Postcode' , 'postcode', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'CountryId')
begin
EXEC sp_RENAME '[general].[tb_a_address].CountryId' , 'country_id', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Longitude')
begin
EXEC sp_RENAME '[general].[tb_a_address].Longitude' , 'longitude', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Latitude')
begin
EXEC sp_RENAME '[general].[tb_a_address].Latitude' , 'latitude', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_a_address' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Status')
begin
EXEC sp_RENAME '[general].[tb_a_address].Status' , 'status', 'COLUMN'
end







if not exists(
SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'tb_cbi_client_basic_information' AND COLUMN_NAME = 'address_id')
begin
alter table [client].[tb_cbi_client_basic_information] add address_id int null
end
GO


IF not exists (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[general].tb_d_dictionary')
    AND type = N'U')
begin
CREATE TABLE [general].[tb_d_dictionary](
	[dictionary_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[category] [nvarchar](50) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[display_name] [nvarchar](500) NOT NULL,
	[parent_id] [int] NULL,
	[tx2id] [int] NULL,
 CONSTRAINT [PK_Dictionary] PRIMARY KEY CLUSTERED 
(
	[dictionary_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_Dictionary_Category_Name] UNIQUE NONCLUSTERED 
(
	[category] ASC,
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionary' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Id')
begin
EXEC sp_RENAME '[general].[tb_d_dictionary].Id' , 'dictionary_id', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionary' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Category')
begin
EXEC sp_RENAME '[general].[tb_d_dictionary].Category' , 'category', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionary' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Name')
begin
EXEC sp_RENAME '[general].[tb_d_dictionary].Name' , 'name', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionary' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'DisplayName')
begin
EXEC sp_RENAME '[general].[tb_d_dictionary].DisplayName' , 'display_name', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionary' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'ParentId')
begin
EXEC sp_RENAME '[general].[tb_d_dictionary].ParentId' , 'parent_id', 'COLUMN'
end








IF not exists (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[general].tb_d_dictionarytranslation')
    AND type = N'U')
begin
	CREATE TABLE [general].[tb_d_dictionarytranslation](
		[dictionary_translation_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[dictionary_id] [int] NOT NULL,
		[language] [nvarchar](5) NOT NULL,
		[display_content] [nvarchar](500) NOT NULL,
		[tx2id] [int] NULL,
	 CONSTRAINT [PK_DictionaryTranslation] PRIMARY KEY CLUSTERED 
	(
		[dictionary_translation_id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_DictionaryTranslation_DictionaryId_Language] UNIQUE NONCLUSTERED 
	(
		[dictionary_id] ASC,
		[language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
end
GO


if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionarytranslation' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Id')
begin
EXEC sp_RENAME '[general].[tb_d_dictionarytranslation].Id' , 'dictionary_translation_id', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionarytranslation' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'DictionaryId')
begin
EXEC sp_RENAME '[general].[tb_d_dictionarytranslation].DictionaryId' , 'dictionary_id', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionarytranslation' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'Language')
begin
EXEC sp_RENAME '[general].[tb_d_dictionarytranslation].Language' , 'language', 'COLUMN'
end

if exists (SELECT 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_d_dictionarytranslation' AND TABLE_SCHEMA='general' AND COLUMN_NAME = 'DisplayContent')
begin
EXEC sp_RENAME '[general].[tb_d_dictionarytranslation].DisplayContent' , 'display_content', 'COLUMN'
end



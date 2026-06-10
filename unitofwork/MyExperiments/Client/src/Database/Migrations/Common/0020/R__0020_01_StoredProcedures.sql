DROP PROCEDURE IF EXISTS [client].[sp_sel_client_list]
GO

CREATE PROCEDURE [client].[sp_sel_client_list]
	-- Add the parameters for the stored procedure here
    @RowCount int = 20,
	@PageOffset int = 0,
	@SearchKeyWord NVARCHAR(100),
	@ClientId INT = 0,
	@TotalCount int = 0 OUTPUT
AS
BEGIN
	IF(@ClientId > 0)
    BEGIN
                SELECT c.[client_id] AS ClientId,
				c.[client_name] AS ClientName,
				c.[identity_code] AS IdentityCode,
				c.[voucher_issuer_id] AS VoucherIssuerId,
				c.[invoice_register_number] AS InvoiceRegisterNumber,
				c.[business_type_id] AS BusinessTypeId,
				c.[status] ,
				c.[security_algorithm] AS SecurityAlgorithm,
				c.[security_key] AS SecurityKey,
				c.[need_notification] AS NeedNotification,
				c.[notification_provider_code_id] AS NotificationProviderCodeId,
				c.[logo_media_id] AS LogoMediaId,
				c.[banner_media_id] AS BannerMediaId,
				c.[email_header_media_id] AS EmailHeaderMediaId,
				c.[email_footer_media_id] AS EmailFooterMediaId,
				c.[can_issue] AS CanIssue,
				c.[mandatory_auto_billing] AS MandatoryAutoBilling,
				c.[invoice_title] AS InvoiceTitle,
				c.[sub_url] AS SubURL,
				c.[email_provider_code] AS EmailProviderCode,
				c.[email_sender_name] AS EmailSenderName,
				c.[email_sender_address] AS EmailSenderAddress,
				c.[apply_email_subject] AS ApplyEmailSubject,
				c.[sms_provider_code] AS SMSProviderCode,
				c.[sms_sender_name] AS SMSSenderName,
				c.[sms_entity_id] AS SMSEntityId,
				c.[sales_email] AS SalesEmail,
				c.[contact_name] AS ContactName,
				c.[contact_email] AS ContactEmail,
				c.[contact_phone] AS ContactPhone,
				c.[Memo],
                c.[description] AS [Description],
				addre.detail_address_line as DetailAddressLine, 
				addre.district as District, 
				addre.city_id as CityId, 
				addre.state_province_id as StateOrProvinceId, 
				addre.postcode as Postcode, 
				addre.country_id as CountryId, 
				addre.longitude as Longitude, 
				addre.latitude as Latitude, 
				addre.[Status] as AddressStatus
			FROM [client].[tb_cbi_client_basic_information] c with(nolock)
			inner join [general].[tb_a_address] addre with(nolock) on c.address_id = addre.address_id
			where c.client_id = @ClientId;

        SET @TotalCount = 1;
    END
    ELSE
    BEGIN
        create table #Tmp 
        (
            ID int NOT NULL
        );
        
        INSERT INTO #Tmp SElECT client_id FROM
        (
        SElECT client_id from tb_cbi_client_basic_information c with(nolock) where c.client_name LIKE N'%'+@SearchKeyWord+'%'
        Union
        SElECT client_id from tb_cbi_client_basic_information c with(nolock) where c.invoice_register_number LIKE N'%'+@SearchKeyWord+'%'
        ) CL

       SELECT c.[client_id] AS ClientId,
				c.[client_name] AS ClientName,
				c.[identity_code] AS IdentityCode,
				c.[voucher_issuer_id] AS VoucherIssuerId,
				c.[invoice_register_number] AS InvoiceRegisterNumber,
				c.[business_type_id] AS BusinessTypeId,
				c.[status] ,
				c.[security_algorithm] AS SecurityAlgorithm,
				c.[security_key] AS SecurityKey,
				c.[need_notification] AS NeedNotification,
				c.[notification_provider_code_id] AS NotificationProviderCodeId,
				c.[logo_media_id] AS LogoMediaId,
				c.[banner_media_id] AS BannerMediaId,
				c.[email_header_media_id] AS EmailHeaderMediaId,
				c.[email_footer_media_id] AS EmailFooterMediaId,
				c.[can_issue] AS CanIssue,
				c.[mandatory_auto_billing] AS MandatoryAutoBilling,
				c.[invoice_title] AS InvoiceTitle,
				c.[sub_url] AS SubURL,
				c.[email_provider_code] AS EmailProviderCode,
				c.[email_sender_name] AS EmailSenderName,
				c.[email_sender_address] AS EmailSenderAddress,
				c.[apply_email_subject] AS ApplyEmailSubject,
				c.[sms_provider_code] AS SMSProviderCode,
				c.[sms_sender_name] AS SMSSenderName,
				c.[sms_entity_id] AS SMSEntityId,
				c.[sales_email] AS SalesEmail,
				c.[contact_name] AS ContactName,
				c.[contact_email] AS ContactEmail,
				c.[contact_phone] AS ContactPhone,
				c.[Memo],                
                c.[description] AS [Description],
				addre.detail_address_line as DetailAddressLine, 
				addre.district as District, 
				addre.city_id as CityId, 
				addre.state_province_id as StateOrProvinceId, 
				addre.postcode as Postcode, 
				addre.country_id as CountryId, 
				addre.longitude as Longitude, 
				addre.latitude as Latitude, 
				addre.[Status] as AddressStatus
        FROM #Tmp t with(nolock) 
        INNER JOIN [client].[tb_cbi_client_basic_information] c with(nolock) on t.Id = c.client_id
		INNER JOIN [general].[tb_a_address] addre with(nolock) on c.address_id = addre.address_id
        Order BY c.client_id 
        OFFSET @PageOffset ROWS
	    FETCH NEXT @RowCount ROWS ONLY

        SELECT @TotalCount = count(1) FROM #Tmp;
        
        DROP TABLE #Tmp
    END
	
END
GO



DROP PROCEDURE IF EXISTS [client].[sp_ins_client]
GO

CREATE PROCEDURE [client].[sp_ins_client]
	    @ClientName nvarchar(100)
        ,@IdentityCode varchar(50)
        ,@InvoiceRegisterNumber nvarchar(100)
	    ,@VoucherIssuerId int = NULL
        ,@BusinessTypeId int = NULL
        ,@Status tinyint
        ,@SecurityAlgorithm tinyint
        ,@SecurityKey varchar(32)
        ,@NeedNotification bit
        ,@NotificationProviderCodeId int = NULL
	    ,@LogoMediaId int = NULL
	    ,@BannerMediaId int = NULL
	    ,@EmailHeaderMediaId int = NULL
        ,@EmailFooterMediaId int = NULL
        ,@CanIssue bit
        ,@MandatoryAutoBilling bit = NULL
        ,@InvoiceTitle nvarchar(100) = NULL
        ,@SubURL nvarchar(6) = NULL
	    ,@EmailProviderCode nvarchar(8) = NULL
        ,@EmailSenderName nvarchar(255) = NULL
        ,@EmailSenderAddress nvarchar(255) = NULL
        ,@ApplyEmailSubject bit = NULL
	    ,@SmsProviderCode nvarchar(8) = NULL
        ,@SMSSenderName nvarchar(255) = NULL
        ,@SmsEntityId nvarchar(30) = NULL
        ,@SalesEmail nvarchar(255) = NULL
        ,@ContactName nvarchar(30) = NULL
        ,@ContactEmail nvarchar(255) = NULL
        ,@ContactPhone nvarchar(50) = NULL
        ,@Memo nvarchar(2000)  = NULL
        ,@Description nvarchar(500)  = NULL		
		, @DetailAddressLine nvarchar(400) = NULL
		, @District nvarchar(400) = NULL
		, @CityId int = NULL
		, @StateOrProvinceId int = NULL
		, @Postcode nvarchar(20) = NULL
		, @CountryId int = NULL
		, @Longitude float = NULL
		, @Latitude float = NULL
		, @AddressStatus int = NULL
	    , @ClientId int output
AS
BEGIN
	SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;
        
		Declare @AddressId int = NULL;

		if (@CountryId is not NULL)
		begin
			insert into [general].[tb_a_address]( 
				[detail_address_line]
				, [district]
				, [city_id]
				, [state_province_id]
				, [postcode]
				, [country_id]
				, [longitude]
				, [latitude]
				, [Status]) values (
				@DetailAddressLine
				, @District
				, @CityId
				, @StateOrProvinceId
				, @Postcode
				, @CountryId
				, @Longitude
				, @Latitude
				, @AddressStatus
				);
			set @AddressId = SCOPE_IDENTITY();
		end

		INSERT INTO [client].[tb_cbi_client_basic_information]
            ([client_name]
            ,[identity_code]
            ,[voucher_issuer_id]
            ,[invoice_register_number]
            ,[business_type_id]
            ,[status]
            ,[security_algorithm]
            ,[security_key]
            ,[need_notification]
            ,[notification_provider_code_id]
            ,[logo_media_id]
            ,[banner_media_id]
            ,[email_header_media_id]
            ,[email_footer_media_id]
            ,[can_issue]
            ,[mandatory_auto_billing]
            ,[invoice_title]
            ,[sub_url]
            ,[email_provider_code]
            ,[email_sender_name]
            ,[email_sender_address]
            ,[apply_email_subject]
            ,[sms_provider_code]
            ,[sms_sender_name]
            ,[sms_entity_id]
            ,[sales_email]
            ,[contact_name]
            ,[contact_email]
            ,[contact_phone]
            ,[Memo]
            ,[description]
			,[address_id])
     VALUES
            (@ClientName 
            ,@IdentityCode 
	        ,@VoucherIssuerId 
	        ,@InvoiceRegisterNumber 
            ,@BusinessTypeId 
            ,@Status 
            ,@SecurityAlgorithm 
            ,@SecurityKey 
            ,@NeedNotification 
            ,@NotificationProviderCodeId 
            ,@LogoMediaId 
	        ,@BannerMediaId 
	        ,@EmailHeaderMediaId 
            ,@EmailFooterMediaId 
	        ,@CanIssue 
            ,@MandatoryAutoBilling 
            ,@InvoiceTitle 
            ,@SubURL 
	        ,@EmailProviderCode 
            ,@EmailSenderName 
            ,@EmailSenderAddress 
	        ,@ApplyEmailSubject 
	        ,@SmsProviderCode 
            ,@SMSSenderName 
            ,@SmsEntityId 
            ,@SalesEmail 
            ,@ContactName 
            ,@ContactEmail 
            ,@ContactPhone 
            ,@Memo
            ,@Description
			,@AddressId)

        COMMIT TRANSACTION;

	set @ClientId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH 

       IF @@TRANCOUNT  > 0
       BEGIN
          ROLLBACK TRANSACTION
       END;

	   THROW;
    END CATCH;

    SET NOCOUNT, XACT_ABORT OFF;
END

GO









DROP PROCEDURE IF EXISTS [client].[sp_upd_client]
GO

CREATE PROCEDURE [client].[sp_upd_client]
         @ClientId int
        ,@ClientName nvarchar(100)
        ,@InvoiceRegisterNumber nvarchar(100)
        ,@VoucherIssuerId int = NULL
        ,@BusinessTypeId int = NULL
        ,@Status tinyint
        ,@SecurityAlgorithm tinyint
        ,@SecurityKey varchar(32)
        ,@NeedNotification bit
        ,@NotificationProviderCodeId int = NULL
        ,@LogoMediaId int = NULL
        ,@BannerMediaId int = NULL
        ,@EmailHeaderMediaId int = NULL
        ,@EmailFooterMediaId int = NULL
        ,@CanIssue bit
        ,@MandatoryAutoBilling bit = NULL
        ,@InvoiceTitle nvarchar(100) = NULL
        ,@SubURL nvarchar(6) = NULL
        ,@EmailProviderCode nvarchar(8) = NULL
        ,@EmailSenderName nvarchar(255) = NULL
        ,@EmailSenderAddress nvarchar(255) = NULL
        ,@ApplyEmailSubject bit = NULL
        ,@SmsProviderCode nvarchar(8) = NULL
        ,@SMSSenderName nvarchar(255) = NULL
        ,@SmsEntityId nvarchar(30) = NULL
        ,@SalesEmail nvarchar(255) = NULL
        ,@ContactName nvarchar(30) = NULL
        ,@ContactEmail nvarchar(255) = NULL
        ,@ContactPhone nvarchar(50) = NULL
        ,@Memo nvarchar(2000) = NULL  
        ,@Description nvarchar(500)  = NULL
		, @DetailAddressLine nvarchar(400) = NULL
		, @District nvarchar(400) = NULL
		, @CityId int = NULL
		, @StateOrProvinceId int = NULL
		, @Postcode nvarchar(20) = NULL
		, @CountryId int = NULL
		, @Longitude float = NULL
		, @Latitude float = NULL
		, @AddressStatus int = NULL
AS
BEGIN
	SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;       
		Declare @AddressId int = NULL;

		UPDATE [client].[tb_cbi_client_basic_information]
		   SET [client_name] = @ClientName
			  ,[voucher_issuer_id] = @VoucherIssuerId
			  ,[invoice_register_number] = @InvoiceRegisterNumber
			  ,[business_type_id] = @BusinessTypeId
			  ,[status] = @Status
			  ,[security_algorithm] = @SecurityAlgorithm
			  ,[security_key] = @SecurityKey
			  ,[need_notification] = @NeedNotification
			  ,[notification_provider_code_id] = @NotificationProviderCodeId
			  ,[logo_media_id] = @LogoMediaId
			  ,[banner_media_id] = @BannerMediaId
			  ,[email_header_media_id] = @EmailHeaderMediaId
			  ,[email_footer_media_id] = @EmailFooterMediaId
			  ,[can_issue] = @CanIssue
			  ,[mandatory_auto_billing] = @MandatoryAutoBilling
			  ,[invoice_title] = @InvoiceTitle
			  ,[sub_url] = @SubURL
			  ,[email_provider_code] = @EmailProviderCode
			  ,[email_sender_name] = @EmailSenderName
			  ,[email_sender_address] = @EmailSenderAddress 
			  ,[apply_email_subject] = @ApplyEmailSubject
			  ,[sms_provider_code] = @SmsProviderCode
			  ,[sms_sender_name] = @SMSSenderName
			  ,[sms_entity_id] = @SmsEntityId
			  ,[sales_email] = @SalesEmail
			  ,[contact_name] = @ContactName
			  ,[contact_email] = @ContactEmail
			  ,[contact_phone] = @ContactPhone
			  ,[Memo] = @Memo
              ,[description] = @Description
		 WHERE client_id = @ClientId;

		 select @AddressId = address_id from [client].[tb_cbi_client_basic_information] with(nolock) where client_id = @ClientId;

		if (@CountryId is not NULL)
		begin
			update [general].[tb_a_address]
				set [detail_address_line] = @DetailAddressLine
				, [district] = @District
				, [city_id] = @CityId
				, [state_province_id] = @StateOrProvinceId
				, [postcode] = @Postcode
				, [country_id] = @CountryId
				, [longitude] = @Longitude
				, [Latitude] = @Latitude
				, [Status] = @AddressStatus
			where address_id = @AddressId
		end

        COMMIT TRANSACTION;
	
		SELECT @@ROWCOUNT;
    END TRY
    BEGIN CATCH 

       IF @@TRANCOUNT  > 0
       BEGIN
          ROLLBACK TRANSACTION
       END;

	   THROW;
    END CATCH;

    SET NOCOUNT, XACT_ABORT OFF;
END

GO


DROP PROCEDURE IF EXISTS [client].[sp_sel_address_by_city]
GO

CREATE PROCEDURE [client].[sp_sel_address_by_city]
	@CityId int
	, @ProvinceId int output
	, @CountryId int output
AS
BEGIN
	SET XACT_ABORT ON;

	select @ProvinceId = province.dictionary_id, @CountryId = country.dictionary_id from [general].[tb_d_dictionary] city with(nolock)
	inner join [general].[tb_d_dictionary] province with(nolock) on city.parent_id = province.dictionary_id AND province.category = 'StateOrProvince'
	inner join [general].[tb_d_dictionary] country with(nolock) on country.dictionary_id = province.parent_id AND country.category = 'Country'
	where city.category = 'City' AND city.dictionary_id = @CityId

	if((@ProvinceId is not null) AND (@CountryId is not null))
	BEGIN
		return 1;
	END
	ELSE
	BEGIN
		return 0;
	END

    SET NOCOUNT, XACT_ABORT OFF;
END

GO
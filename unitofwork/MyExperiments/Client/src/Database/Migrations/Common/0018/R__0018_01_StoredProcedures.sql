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
AS
BEGIN
	SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;
        
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
            ,[description])
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
            ,@Description)

        COMMIT TRANSACTION;

	select SCOPE_IDENTITY();
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
        ,@Memo nvarchar(2000) = NULL  
        ,@Description nvarchar(500)  = NULL
AS
BEGIN
	SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;
        
		UPDATE [client].[tb_cbi_client_basic_information]
		   SET [client_name] = @ClientName
			  ,[identity_code] = @IdentityCode
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



DROP PROCEDURE IF EXISTS [client].[sp_sel_client_list]
GO

CREATE PROCEDURE [client].[sp_sel_client_list]
	-- Add the parameters for the stored procedure here
    @RowCount int = 20,
	@PageOffset int = 0,
	@SearchKeyWord VARCHAR(100),
	@ClientId INT = 0,
	@TotalCount int = 0 OUTPUT
AS
BEGIN
	IF(@ClientId > 0)
    BEGIN
                SELECT [client_id] AS ClientId,
				[client_name] AS ClientName,
				[identity_code] AS IdentityCode,
				[voucher_issuer_id] AS VoucherIssuerId,
				[invoice_register_number] AS InvoiceRegisterNumber,
				[business_type_id] AS BusinessTypeId,
				[status] ,
				[security_algorithm] AS SecurityAlgorithm,
				[security_key] AS SecurityKey,
				[need_notification] AS NeedNotification,
				[notification_provider_code_id] AS NotificationProviderCodeId,
				[logo_media_id] AS LogoMediaId,
				[banner_media_id] AS BannerMediaId,
				[email_header_media_id] AS EmailHeaderMediaId,
				[email_footer_media_id] AS EmailFooterMediaId,
				[can_issue] AS CanIssue,
				[mandatory_auto_billing] AS MandatoryAutoBilling,
				[invoice_title] AS InvoiceTitle,
				[sub_url] AS SubURL,
				[email_provider_code] AS EmailProviderCode,
				[email_sender_name] AS EmailSenderName,
				[email_sender_address] AS EmailSenderAddress,
				[apply_email_subject] AS ApplyEmailSubject,
				[sms_provider_code] AS SMSProviderCode,
				[sms_sender_name] AS SMSSenderName,
				[sms_entity_id] AS SMSEntityId,
				[sales_email] AS SalesEmail,
				[contact_name] AS ContactName,
				[contact_email] AS ContactEmail,
				[contact_phone] AS ContactPhone,
				[Memo],
                [description] AS [Description]
			FROM [client].[tb_cbi_client_basic_information] c with(nolock)
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
        SElECT client_id from tb_cbi_client_basic_information c with(nolock) where c.client_name LIKE '%'+@SearchKeyWord+'%'
        Union
        SElECT client_id from tb_cbi_client_basic_information c with(nolock) where c.invoice_register_number LIKE '%'+@SearchKeyWord+'%'
        ) CL

       SELECT [client_id] AS ClientId,
				[client_name] AS ClientName,
				[identity_code] AS IdentityCode,
				[voucher_issuer_id] AS VoucherIssuerId,
				[invoice_register_number] AS InvoiceRegisterNumber,
				[business_type_id] AS BusinessTypeId,
				[status] ,
				[security_algorithm] AS SecurityAlgorithm,
				[security_key] AS SecurityKey,
				[need_notification] AS NeedNotification,
				[notification_provider_code_id] AS NotificationProviderCodeId,
				[logo_media_id] AS LogoMediaId,
				[banner_media_id] AS BannerMediaId,
				[email_header_media_id] AS EmailHeaderMediaId,
				[email_footer_media_id] AS EmailFooterMediaId,
				[can_issue] AS CanIssue,
				[mandatory_auto_billing] AS MandatoryAutoBilling,
				[invoice_title] AS InvoiceTitle,
				[sub_url] AS SubURL,
				[email_provider_code] AS EmailProviderCode,
				[email_sender_name] AS EmailSenderName,
				[email_sender_address] AS EmailSenderAddress,
				[apply_email_subject] AS ApplyEmailSubject,
				[sms_provider_code] AS SMSProviderCode,
				[sms_sender_name] AS SMSSenderName,
				[sms_entity_id] AS SMSEntityId,
				[sales_email] AS SalesEmail,
				[contact_name] AS ContactName,
				[contact_email] AS ContactEmail,
				[contact_phone] AS ContactPhone,
				[Memo],                
                [description] AS [Description]
        FROM #Tmp t with(nolock) 
        INNER JOIN [client].[tb_cbi_client_basic_information] c with(nolock) on t.Id = c.client_id
        Order BY c.client_id 
        OFFSET @PageOffset ROWS
	    FETCH NEXT @RowCount ROWS ONLY

        SELECT @TotalCount = count(1) FROM #Tmp;
        
        DROP TABLE #Tmp
    END
	
END
GO



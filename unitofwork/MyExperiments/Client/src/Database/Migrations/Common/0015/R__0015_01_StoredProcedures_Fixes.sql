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
				[Memo] 
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
				[Memo] 
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



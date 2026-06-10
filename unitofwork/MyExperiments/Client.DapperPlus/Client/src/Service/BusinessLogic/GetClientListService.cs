using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Repository;
using System.Data;
using TXC.Proto.Client;

namespace Service.BusinessLogic
{
    public interface IGetClientListService
    {
        public Task<ProtoBaseResponse> GetClientList(GetClientListRequest request);
    }
    public class GetClientListService : IGetClientListService
    {
        private IDbConnection _dbConnection;
        private readonly IClientDBService _clientDBService;
        private readonly ICoreService _coreService;
        public GetClientListService(
            IClientDBService clientDBService,
            ICoreService coreService)
        {
            _clientDBService = clientDBService;
            _coreService = coreService;
        }

        public async Task<ProtoBaseResponse> GetClientList(GetClientListRequest request)
        {
            try 
            {
                if (request.TenantId <= 0)
                    return new ProtoBaseResponse() { Success = false, Message = "TenantBasicInfoId header required" };

                if (string.IsNullOrEmpty(request.TenantName))
                    return new ProtoBaseResponse() { Success = false, Message = "TenantName header required" };

                // checkers for default values in pagination
                if (request.PageNumber == 0 || request.PageNumber == null)
                    request.PageNumber = 1;
                if (request.RowCount == 0 || request.RowCount == null)
                    request.RowCount = 20;

                // initialize db connection
                var conn = await _coreService.GetDBConnection(request.TenantId);

                if (!conn.Success)
                    return new ProtoBaseResponse() { Success = false, Message = "Error in Tenant DB" };

                _dbConnection = conn.Data;
                GetClientListModel model = new GetClientListModel
                {
                    SearchKeyWord = request.SearchKeyword,
                    ClientId = request.ClientId,
                    PageNumber = request.PageNumber,
                    RowCount = request.RowCount,
                };
                var dbResult = await _clientDBService.GetClientAsync(model, _dbConnection);

                GetClientListResponse response = new GetClientListResponse();
                List<Domain.Entities.ClientAddress> _clientList = new List<Domain.Entities.ClientAddress>();
                _clientList = dbResult.Item2;
                foreach (var item in _clientList)
                {
                    ClientListItem client = new ClientListItem
                    {
                        SalesEmail = item.Client.Sales_Email,
                        SecurityAlgorithm = item.Client.Security_Algorithm,
                        SecurityKey = item.Client.Security_Key,
                        SmsEntityId = item.Client.Sms_Entity_Id,
                        Description = item.Client.Description,
                        SmsProviderCode = item.Client.SMS_Provider_Code,
                        SmsSenderName = item.Client.SMS_Sender_Name,
                        StateOrProvinceId = item.Address.State_Province_Id,
                        Status = item.Client.Status,
                        SubUrl = item.Client.Sub_URL,
                        AddressStatus = item.Address.Status ?? 0,
                        ApplyEmailSubject = item.Client.Apply_Email_Subject ?? false,
                        DetailAddressLine = item.Address.Detail_Address_Line,
                        EmailSenderAddress = item.Client.Email_Sender_Address,
                        District = item.Address.District,
                        EmailSenderName = item.Client.Email_Sender_Name,
                        BannerMediaId = item.Client.Banner_Media_Id,
                        BusinessTypeId = item.Client.Business_Type_Id,
                        CanIssue = item.Client.Can_Issue,
                        MandatoryAutoBilling = item.Client.Mandatory_Auto_Billing ?? false,
                        CityId = item.Address.City_Id,
                        ClientId = item.Client.Client_Id,
                        ClientName = item.Client.Client_Name,
                        ContactEmail = item.Client.Contact_Email,
                        ContactName = item.Client.Contact_Name,
                        ContactPhone = item.Client.Contact_Phone,
                        CountryId = item.Address.Country_Id,
                        EmailFooterMediaId = item.Client.Email_Footer_Media_Id,
                        EmailHeaderMediaId = item.Client.Email_Header_Media_Id,
                        EmailProviderCode = item.Client.Email_Provider_Code,
                        IdentityCode = item.Client.Identity_Code,
                        InvoiceRegisterNumber = item.Client.Invoice_Register_Number,
                        InvoiceTitle = item.Client.Invoice_Title,
                        Latitude = (float)(item.Address.Latitude ?? 0),
                        Longitude = (float)(item.Address.Longitude ?? 0),
                        LogoMediaId = item.Client.Logo_Media_Id,
                        Memo = item.Client.Memo,
                        NeedNotification = item.Client.Need_Notification,
                        NotificationProviderCodeId = item.Client.Notification_Provider_Code_Id,
                        Postcode = item.Address.Postcode,
                        VoucherIssuerId = item.Client.Voucher_Issuer_Id,
                    };
                    response.ClientDtos.Add(client);
                }

                response.TotalCount = dbResult.Item1;
                return new ProtoBaseResponse
                {
                    Success = true,
                    Message = "Success",
                    Data = Any.Pack(response)
                };
            } 
            catch (Exception) 
            {
                throw;
            }
        }
    }
}

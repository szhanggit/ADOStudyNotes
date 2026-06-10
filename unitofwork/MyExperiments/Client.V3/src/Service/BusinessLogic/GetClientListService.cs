using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Repository.Dapper;
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
        private readonly IClientOperation _clientOperation;
        private readonly ICoreService _coreService;
        public GetClientListService(
            IClientOperation clientOperation,
            ICoreService coreService)
        {
            _clientOperation = clientOperation;
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
                var dbResult = await _clientOperation.GetClientAsync(model, _dbConnection);

                GetClientListResponse response = new GetClientListResponse();
                List<Domain.Entities.Client> _clientList = new List<Domain.Entities.Client>();
                _clientList = dbResult.Item2;
                foreach (var item in _clientList)
                {
                    ClientListItem client = new ClientListItem
                    {
                        SalesEmail = item.SalesEmail,
                        SecurityAlgorithm = item.SecurityAlgorithm,
                        SecurityKey = item.SecurityKey,
                        SmsEntityId = item.SmsEntityId,
                        Description = item.Description,
                        SmsProviderCode = item.SMSProviderCode,
                        SmsSenderName = item.SMSSenderName,
                        StateOrProvinceId = item.StateOrProvinceId,
                        Status = item.Status,
                        SubUrl = item.SubURL,
                        AddressStatus = item.AddressStatus,
                        ApplyEmailSubject = item.ApplyEmailSubject,
                        DetailAddressLine = item.DetailAddressLine,
                        EmailSenderAddress = item.EmailSenderAddress,
                        District = item.District,
                        EmailSenderName = item.EmailSenderName,
                        BannerMediaId = item.BannerMediaId,
                        BusinessTypeId = item.BusinessTypeId,
                        CanIssue = item.CanIssue,
                        MandatoryAutoBilling = item.MandatoryAutoBilling,
                        CityId = item.CityId,
                        ClientId = item.ClientId,
                        ClientName = item.ClientName,
                        ContactEmail = item.ContactEmail,
                        ContactName = item.ContactName,
                        ContactPhone = item.ContactPhone,
                        CountryId = item.CountryId,
                        EmailFooterMediaId = item.EmailFooterMediaId,
                        EmailHeaderMediaId = item.EmailHeaderMediaId,
                        EmailProviderCode = item.EmailProviderCode,
                        IdentityCode = item.IdentityCode,
                        InvoiceRegisterNumber = item.InvoiceRegisterNumber,
                        InvoiceTitle = item.InvoiceTitle,
                        Latitude = (float)(item.Latitude ?? 0),
                        Longitude = (float)(item.Longitude ?? 0),
                        LogoMediaId = item.LogoMediaId,
                        Memo = item.Memo,
                        NeedNotification = item.NeedNotification,
                        NotificationProviderCodeId = item.NotificationProviderCodeId,
                        Postcode = item.Postcode,
                        VoucherIssuerId = item.VoucherIssuerId,
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

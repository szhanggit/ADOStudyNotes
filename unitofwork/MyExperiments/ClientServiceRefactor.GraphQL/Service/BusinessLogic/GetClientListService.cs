using Domain.Models;
using FluentValidation;
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
        private readonly IClientAddressFetchingGraphQLService _clientAddressFetchingGraphQLService;
        private readonly IValidator<GetClientListRequest> _validator;

        public GetClientListService(
            IClientDBService clientDBService,
            ICoreService coreService,
            IClientAddressFetchingGraphQLService clientAddressFetchingGraphQLService,
            IValidator<GetClientListRequest> validator)
        {
            _clientDBService = clientDBService;
            _coreService = coreService;
            _clientAddressFetchingGraphQLService = clientAddressFetchingGraphQLService;
            _validator = validator;
        }

        public async Task<ProtoBaseResponse> GetClientList(GetClientListRequest request)
        {
            ProtoBaseResponse response = null;
            try 
            {
                if (request.TenantId <= 0)
                    return new ProtoBaseResponse() { Success = false, Message = "TenantBasicInfoId header required" };

                if (string.IsNullOrEmpty(request.TenantName))
                    return new ProtoBaseResponse() { Success = false, Message = "TenantName header required" };

                var vresult = await _validator.ValidateAsync(request);

                if (!vresult.IsValid)
                {
                    string _errorMessage = vresult.Errors.FirstOrDefault().ErrorMessage;
                    return new ProtoBaseResponse() { Success = false, Message = _errorMessage };
                }

                // checkers for default values in pagination
                if (request.PageNumber == 0 || request.PageNumber == null)
                    request.PageNumber = 1;
                if (request.RowCount == 0 || request.RowCount == null)
                    request.RowCount = 20;

                GetClientListModel searchModel = new GetClientListModel
                {
                    TenantId = request.TenantId,
                    SearchKeyWord = request.SearchKeyword,
                    ClientId = request.ClientId,
                    PageNumber = request.PageNumber,
                    RowCount = request.RowCount,
                };

                if (searchModel.ClientId.HasValue)
                {
                    response = await _clientAddressFetchingGraphQLService.GetClientsByIdAsync(searchModel);
                }
                else if (!string.IsNullOrEmpty(request.SearchKeyword) && !string.IsNullOrWhiteSpace(request.SearchKeyword))
                {
                    response = await _clientAddressFetchingGraphQLService.GetClientsBySearchKeyAsync(searchModel);
                }
                else
                {
                    response = await _clientAddressFetchingGraphQLService.GetClientsAsync(searchModel);
                }

                return response;
            } 
            catch (Exception) 
            {
                throw;
            }
        }
    }
}

using Domain.Models;
using Domain.Models.Response;
using Google.Protobuf.WellKnownTypes;
using GraphQL;
using Service.Utility;
using Service.Utility.GraphQLClient;
using System.Text;
using TXC.Proto.Client;

namespace Service.BusinessLogic
{
    public interface IClientFetchingGraphQLService
    {
        Task<ProtoBaseResponse> GetClientsAsync(GetClientListModel request);
        Task<ProtoBaseResponse> GetClientsBySearchKeyAsync(GetClientListModel request);
        Task<ProtoBaseResponse> GetClientsByIdAsync(GetClientListModel request);
        Task<TXC.Proto.Client.GetClientListResponse> GetClientsByNameAsync(GetClientListModel request);
        Task<ClientModel> GetClientsByCodeAsync(GetClientListModel request);
    }
    public class ClientFetchingGraphQLService : IClientFetchingGraphQLService
    {
        private IClientGraphQLClient _graphQLClient;
        private IClientHelperService _clientHelperService;
        private IObjectConvertingService _objectConvertingService;
        public ClientFetchingGraphQLService(IClientGraphQLClient graphQLClient, IClientHelperService clientHelperService, IObjectConvertingService objectConvertingService)
        {
            _graphQLClient = graphQLClient;
            _clientHelperService = clientHelperService;
            _objectConvertingService = objectConvertingService;
        }

        public async Task<ProtoBaseResponse> GetClientsAsync(GetClientListModel request)
        {
            int? TotalCount = 0;
            TXC.Proto.Client.GetClientListResponse response = new TXC.Proto.Client.GetClientListResponse();
            var graphqlClient = _graphQLClient.GetGraphQLClient(request.TenantId);

            int skip = _clientHelperService.GetSkipNum(request.PageNumber, request.RowCount);
            int take = (request.RowCount ?? 0);
            StringBuilder _querySB = new StringBuilder();
            _querySB.AppendLine("query{");
            _querySB.AppendLine($"client(skip: {skip}, take: {take})");                         
            _querySB.AppendLine(@"{items {
	                                addressId,
	                                applyEmailSubject,
	                                bannerMediaId,
	                                businessTypeId,
	                                canIssue,	
                                    clientName,
	                                contactEmail,
	                                contactPhone,
	                                description,
	                                emailFooterMediaId,
	                                emailHeaderMediaId,
	                                emailProviderCode,
	                                emailSenderAddress,
	                                emailSenderName,	
                                    id,
                                    identityCode,
                                    invoiceRegisterNumber,
                                    invoiceTitle,
	                                logoMediaId,
	                                mandatoryAutoBilling,
	                                memo,
	                                needNotification,
	                                notificationProviderCodeId,
	                                salesEmail,
	                                securityAlgorithm,
	                                securityKey,
	                                smsEntityId,
	                                sMSProviderCode,
	                                sMSSenderName,
	                                status,
	                                subURL,
	                                timeStamp,
	                                voucherIssuerId                            
                              }, totalCount     
                            }
                        }");

            var query = new GraphQLRequest
            {
                Query = _querySB.ToString(),
            };

            if (graphqlClient != null)
            {
                GraphQLResponse<GetClientGraphQLResponse> clientFetchingResult = await graphqlClient.SendQueryAsync<GetClientGraphQLResponse>(query, default);

                TotalCount = clientFetchingResult?.Data?.Client?.TotalCount;
                if (TotalCount == 0)
                {
                    return new ProtoBaseResponse
                    {
                        Success = false,
                        Message = "No client items returned"
                    };
                }
                List<ClientModel> _clientList = clientFetchingResult?.Data?.Client?.Items;

                if (_clientList != null)
                {
                    foreach (var item in _clientList)
                    {
                        ClientListItem client = _objectConvertingService.ConvertClientModelToClientListItemWithoutAddress(item);
                        response.ClientDtos.Add(client);
                    }
                }

                response.TotalCount = TotalCount??0;
            }

            return new ProtoBaseResponse
            {
                Success = true,
                Message = "success",
                Data = Any.Pack(response)
            };
        }

        public async Task<ProtoBaseResponse> GetClientsBySearchKeyAsync(GetClientListModel request)
        {
            int? TotalCount = 0;
            TXC.Proto.Client.GetClientListResponse response = new TXC.Proto.Client.GetClientListResponse();
            var graphqlClient = _graphQLClient.GetGraphQLClient(request.TenantId);

            int skip = _clientHelperService.GetSkipNum(request.PageNumber, request.RowCount);
            int take = (request.RowCount ?? 0);
            StringBuilder _querySB = new StringBuilder();
            _querySB.AppendLine("query{");
            _querySB.AppendLine($"clientBySearchKey(searchKey: \"{request.SearchKeyWord}\", skip: {skip}, take: {take})");
            _querySB.AppendLine(@"{items {
	                                addressId,
	                                applyEmailSubject,
	                                bannerMediaId,
	                                businessTypeId,
	                                canIssue,	
                                    clientName,
	                                contactEmail,
	                                contactPhone,
	                                description,
	                                emailFooterMediaId,
	                                emailHeaderMediaId,
	                                emailProviderCode,
	                                emailSenderAddress,
	                                emailSenderName,	
                                    id,
                                    identityCode,
                                    invoiceRegisterNumber,
                                    invoiceTitle,
	                                logoMediaId,
	                                mandatoryAutoBilling,
	                                memo,
	                                needNotification,
	                                notificationProviderCodeId,
	                                salesEmail,
	                                securityAlgorithm,
	                                securityKey,
	                                smsEntityId,
	                                sMSProviderCode,
	                                sMSSenderName,
	                                status,
	                                subURL,
	                                timeStamp,
	                                voucherIssuerId                            
                              }, totalCount     
                            }
                        }");

            var query = new GraphQLRequest
            {
                Query = _querySB.ToString(),
            };

            if (graphqlClient != null)
            {
                GraphQLResponse<GetClientGraphQLResponse> clientFetchingResult = await graphqlClient.SendQueryAsync<GetClientGraphQLResponse>(query, default);

                TotalCount = clientFetchingResult?.Data?.ClientBySearchKey?.TotalCount;
                if (TotalCount == 0)
                {
                    return new ProtoBaseResponse
                    {
                        Success = false,
                        Message = "No client items returned"
                    };
                }
                List<ClientModel> _clientList = clientFetchingResult?.Data?.ClientBySearchKey?.Items;

                if (_clientList != null)
                {
                    foreach (var item in _clientList)
                    {
                        ClientListItem client = _objectConvertingService.ConvertClientModelToClientListItemWithoutAddress(item);
                        response.ClientDtos.Add(client);
                    }
                }

                response.TotalCount = TotalCount ?? 0;
            }

            return new ProtoBaseResponse
            {
                Success = true,
                Message = "success",
                Data = Any.Pack(response)
            };
        }

        public async Task<ProtoBaseResponse> GetClientsByIdAsync(GetClientListModel request)
        {
            int? TotalCount = 0;
            ClientListItem ClientItem = null;
            TXC.Proto.Client.GetClientListResponse response = new TXC.Proto.Client.GetClientListResponse();
            var graphqlClient = _graphQLClient.GetGraphQLClient(request.TenantId);

            StringBuilder _querySB = new StringBuilder();
            _querySB.AppendLine("query{");
            _querySB.AppendLine($"clientByID(clientID: {request.ClientId})");
            _querySB.AppendLine(@"{ addressId,
	                                applyEmailSubject,
	                                bannerMediaId,
	                                businessTypeId,
	                                canIssue,	
                                    clientName,
	                                contactEmail,
	                                contactPhone,
	                                description,
	                                emailFooterMediaId,
	                                emailHeaderMediaId,
	                                emailProviderCode,
	                                emailSenderAddress,
	                                emailSenderName,	
                                    id,
                                    identityCode,
                                    invoiceRegisterNumber,
                                    invoiceTitle,
	                                logoMediaId,
	                                mandatoryAutoBilling,
	                                memo,
	                                needNotification,
	                                notificationProviderCodeId,
	                                salesEmail,
	                                securityAlgorithm,
	                                securityKey,
	                                smsEntityId,
	                                sMSProviderCode,
	                                sMSSenderName,
	                                status,
	                                subURL,
	                                timeStamp,
	                                voucherIssuerId    
                            }
                        }");

            var query = new GraphQLRequest
            {
                Query = _querySB.ToString(),
            };

            if (graphqlClient != null)
            {
                GraphQLResponse<GetClientByIdGraphQLResponse> clientFetchingResult = await graphqlClient.SendQueryAsync<GetClientByIdGraphQLResponse>(query, default);
                ClientModel _client = clientFetchingResult?.Data?.ClientByID.FirstOrDefault();
                ClientItem = _objectConvertingService.ConvertClientModelToClientListItem(_client);
            }

            response.ClientDtos.Add(ClientItem);
            response.TotalCount = 1;

            return new ProtoBaseResponse
            {
                Success = true,
                Message = "success",
                Data = Any.Pack(response)
            };
        }

        public async Task<TXC.Proto.Client.GetClientListResponse> GetClientsByNameAsync(GetClientListModel request)
        {
            int? TotalCount = 0;
            ClientListItem ClientItem = null;
            TXC.Proto.Client.GetClientListResponse response = new TXC.Proto.Client.GetClientListResponse();
            var graphqlClient = _graphQLClient.GetGraphQLClient(request.TenantId);

            StringBuilder _querySB = new StringBuilder();
            _querySB.AppendLine("query{");
            _querySB.AppendLine($"clientByName(clientName: \"{request.SearchKeyWord}\")");
            _querySB.AppendLine(@"{ addressId,
	                                applyEmailSubject,
	                                bannerMediaId,
	                                businessTypeId,
	                                canIssue,	
                                    clientName,
	                                contactEmail,
	                                contactPhone,
	                                description,
	                                emailFooterMediaId,
	                                emailHeaderMediaId,
	                                emailProviderCode,
	                                emailSenderAddress,
	                                emailSenderName,	
                                    id,
                                    identityCode,
                                    invoiceRegisterNumber,
                                    invoiceTitle,
	                                logoMediaId,
	                                mandatoryAutoBilling,
	                                memo,
	                                needNotification,
	                                notificationProviderCodeId,
	                                salesEmail,
	                                securityAlgorithm,
	                                securityKey,
	                                smsEntityId,
	                                sMSProviderCode,
	                                sMSSenderName,
	                                status,
	                                subURL,
	                                timeStamp,
	                                voucherIssuerId    
                            }
                        }");

            var query = new GraphQLRequest
            {
                Query = _querySB.ToString(),
            };

            if (graphqlClient != null)
            {
                GraphQLResponse<GetClientByNameGraphQLResponse> clientFetchingResult = await graphqlClient.SendQueryAsync<GetClientByNameGraphQLResponse>(query, default);
                Domain.Entities.Client _client = clientFetchingResult?.Data?.ClientByName?.FirstOrDefault();
                if (_client == null)
                {
                    response.TotalCount = 0;
                    return response;
                }
                ClientItem = _objectConvertingService.ConvertClientEntityToClientListItem(_client);               
            }



            response.ClientDtos.Add(ClientItem);
            response.TotalCount = 1;

            return response;
        }

        public async Task<ClientModel> GetClientsByCodeAsync(GetClientListModel request)
        {
            int? TotalCount = 0;
            ClientModel _client = null;
            TXC.Proto.Client.GetClientListResponse response = new TXC.Proto.Client.GetClientListResponse();
            var graphqlClient = _graphQLClient.GetGraphQLClient(request.TenantId);

            StringBuilder _querySB = new StringBuilder();
            _querySB.AppendLine("query{");
            _querySB.AppendLine($"clientByCode(clientCode: \"{request.SearchKeyWord}\")");
            _querySB.AppendLine(@"{ addressId,
	                                applyEmailSubject,
	                                bannerMediaId,
	                                businessTypeId,
	                                canIssue,	
                                    clientName,
	                                contactEmail,
	                                contactPhone,
	                                description,
	                                emailFooterMediaId,
	                                emailHeaderMediaId,
	                                emailProviderCode,
	                                emailSenderAddress,
	                                emailSenderName,	
                                    id,
                                    identityCode,
                                    invoiceRegisterNumber,
                                    invoiceTitle,
	                                logoMediaId,
	                                mandatoryAutoBilling,
	                                memo,
	                                needNotification,
	                                notificationProviderCodeId,
	                                salesEmail,
	                                securityAlgorithm,
	                                securityKey,
	                                smsEntityId,
	                                sMSProviderCode,
	                                sMSSenderName,
	                                status,
	                                subURL,
	                                timeStamp,
	                                voucherIssuerId    
                            }
                        }");

            var query = new GraphQLRequest
            {
                Query = _querySB.ToString(),
            };

            if (graphqlClient != null)
            {
                GraphQLResponse<GetClientByCodeGraphQLResponse> clientFetchingResult = await graphqlClient.SendQueryAsync<GetClientByCodeGraphQLResponse>(query, default);
                _client = clientFetchingResult?.Data?.ClientByCode?.FirstOrDefault();                
            }


            return _client;
        }
    }
}

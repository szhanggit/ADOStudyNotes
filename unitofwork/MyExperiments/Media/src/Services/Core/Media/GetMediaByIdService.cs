using AutoMapper;
using Dapper;
using Domain.Models.ConfigOptions;
using Google.Protobuf.WellKnownTypes;
using GraphQL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Services.GraphQLResponse;
using Services.Utility.GraphQLClient;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Proto.Media;
using static Repository.MediaUnit;

namespace Services.Core
{
    public interface IGetMediaByIdService
    {
        Task<ProtoBaseResponse> GetMediaId(GetMediaByIdRequest request);
    }
    public class GetMediaByIdService : ServiceHandlerBase, IGetMediaByIdService
    {
        private readonly CdnConfiguration _cdnConfig;
        IMediaGraphQLClient _graphQLClient;
        public GetMediaByIdService(ITenantDbConnection tenantDbConnection,
            IOptions<CdnConfiguration> cdnConfig,
            IDapperOperation dapperOperation,
            IMediaGraphQLClient graphQLClient) : base(tenantDbConnection, dapperOperation)
        {
            _cdnConfig = cdnConfig.Value;
            _graphQLClient = graphQLClient;
        }

        public async Task<ProtoBaseResponse> GetMediaId(GetMediaByIdRequest request)
        {
            
            var result = new GetMediaByIdResponse();
            var graphqlClient = _graphQLClient.GetGraphQLClient(request.TenantId);

            var query = new GraphQLRequest
            {
                Query = "query{" +
                        "mediaById(mediaId: " + request.MediaId + ") {" +
                        "mediaId," +
                        "fileName," +
                        "fileContentType," +
                        "nodeUrl," +
                        "account," +
                        "blobName," +
                        "type," +
                        "width," +
                        "height," +
                        "keyword" +
                        "}" +
                        "}"
            };

            if (graphqlClient != null)
            {
               
                var medias = await graphqlClient.SendQueryAsync<GetMediaByIdGraphQLResponse>(query, default);
                var d = medias.Data.Media;
                if (d != null)
                {
                    result.MediaId = d.Media_Id;
                    result.FileName = d.File_Name;
                    result.FileContentType = d.File_Content_Type;
                    result.KeyWord = d.Keyword;
                    result.Height = d.Height;
                    result.Width = d.Width;
                    result.Url = $"{_cdnConfig.ImageCdnUri}{d.Node_Url}";
                    result.BlobName = d.Blob_Name;
                    result.MediaCategory = d.Type;
                }
            }

            return new ProtoBaseResponse
            {
                Success = true,
                Message = "success",
                Data = Any.Pack(result)
            };
            
           
        }
    }
}

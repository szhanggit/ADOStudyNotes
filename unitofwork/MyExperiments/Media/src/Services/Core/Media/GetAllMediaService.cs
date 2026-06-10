using AutoMapper;
using Dapper;
using Domain.EnumList;
using Domain.Models.ConfigOptions;
using Domain.Models.Request;
using Domain.Models.Response;
using Google.Protobuf.WellKnownTypes;
using GraphQL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

using Services.GraphQLResponse;
using Services.Utility;
using Services.Utility.GraphQLClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Proto.Media;
using e = Entities;

namespace Services.Core
{
    public interface IGetAllMediaService
    {
        Task<ProtoBaseResponse> GetAllMedia(GetAllMediaRequest request);
    }
    public class GetAllMediaService : ServiceHandlerBase, IGetAllMediaService
    {

        private readonly CdnConfiguration _cdnConfig;
        IMediaGraphQLClient _graphQLClient;
        public GetAllMediaService(ITenantDbConnection tenantDbConnection,
            IOptions<CdnConfiguration> cdnConfig,
            IDapperOperation dapperOperation,
            IMediaGraphQLClient graphQLClient
            ) : base(tenantDbConnection, dapperOperation)
        {
            _cdnConfig = cdnConfig.Value;
            _graphQLClient = graphQLClient;
        }

        public async Task<ProtoBaseResponse> GetAllMedia(GetAllMediaRequest request)
        {
            GetAllMediaResponse response = new GetAllMediaResponse();
            var graphqlClient = _graphQLClient.GetGraphQLClient(request.TenantId);

            var query = new GraphQLRequest
            {
                Query= @"query{
                            media {
                            items {
                                mediaId,
                                fileName,
                                fileContentType,
                                nodeUrl,
                                account,
                                blobName,
                                type,
                                width,
                                height,
                                keyword
                            }    
                            }
                        }"
            };

            if (graphqlClient != null)
            {
                var medias = await graphqlClient.SendQueryAsync<GetAllMediaGraphQLResponse>(query, default);
                if (medias.Data.media.Items.Any())
                {
                    response.MediaItems.AddRange(medias.Data.media.Items.Select(s => new GetAllMediaItem
                    {
                        MediaId = s.Media_Id,
                        FileName = s.File_Name,
                        KeyWord = s.Keyword,
                        Height = s.Height,
                        Width = s.Width,
                        Url = $"{_cdnConfig.ImageCdnUri}{s.Node_Url}",
                        BlobName = s.Blob_Name,
                        MediaCategory = s.Type
                    }));
                }
            }
                
            return new ProtoBaseResponse
            {
                Success = true,
                Message = "success",
                Data = Any.Pack(response)
            };

        }
    }
}

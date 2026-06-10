using Dapper;
using Domain.Dto;
using Domain.Models.ConfigOptions;
using Domain.Models.Response;
using Microsoft.Extensions.Options;
using TXC.Common.Data.TenantDbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;
using System.Diagnostics.CodeAnalysis;

namespace Services.Queries.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetImageMediaListQueryHandler : ServiceHandlerBase ,IRequestListHandlerWrapper<GetImageMediaListQuery, GetImageMediaListResponse>
    {

        private IDbConnection _dbConnection;
        private readonly CdnConfiguration _cdnConfig;
        public GetImageMediaListQueryHandler(ITenantDbConnection tenantDbConnection
                                            ,IOptions<CdnConfiguration> cdnConfig
                                            ,IDapperOperation dapperOperation
                                            ) : base (tenantDbConnection, dapperOperation)
        {
            _cdnConfig = cdnConfig.Value;
        }
        public async Task<Response<GetImageMediaListResponse>> Handle(GetImageMediaListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // initialize db connection
                var conn = await _tenantDbConnection.GetTenantDbConnection(false, cancellationToken);

                if (!conn.Success)
                    return Response.Fail<GetImageMediaListResponse>("Error in Tenant DB", null);

                _dbConnection = conn.Data;

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RowCount", request.RowCount, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@PageOffset", request.GetPageOffset(), DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Type", (int)request.Type, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@SearchKeyWord", request.SearchKeyword, DbType.String, ParameterDirection.Input);
                parameters.Add("@TotalCount", 0, DbType.Int32, ParameterDirection.Output);

                CommandDefinition commandDefinition = new CommandDefinition(SqlMediaList(), commandType: CommandType.Text,
                                                                            parameters: parameters, cancellationToken: cancellationToken);

                var dbResult = await _dapperOperation.ProcessSql<SelectMany<MediaDto>, IEnumerable<MediaDto>>(_dbConnection, commandDefinition);

                if (dbResult.Count() != 0)
                {
                    dbResult.ToList().ForEach(p =>
                    {
                        p.NodeUrl = $"{_cdnConfig.ImageCdnUri}{p.NodeUrl}";
                    });
                }

                GetImageMediaListResponse response = new GetImageMediaListResponse
                {
                    MediaDtos = dbResult.ToList(),
                    TotalCount = parameters.Get<int>("@TotalCount")
                };

                return Response.Success("sucess", response);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string SqlMediaList()
        {
            return @"
                SElECT
                	media_id MediaId,
                	[file_name] AS [FileName],
                	keyword AS Keyword,
                	height AS Height,
                	width AS Width,
                	blob_name AS BlobName,
                	[type] AS [Type],
                	node_url AS NodeUrl
                FROM media.tb_m_media
                WHERE [type] = @Type
                AND keyword LIKE IIF(@SearchKeyWord IS NULL, keyword, N'%'+@SearchKeyWord+'%' )
                Order BY MediaId
                OFFSET @PageOffset ROWS
                FETCH NEXT @RowCount ROWS ONLY
                
                SET @TotalCount = @@ROWCOUNT
             ";
        }
    }
}

using Dapper;
using Domain.Dto;
using Domain.Models.ConfigOptions;
using Microsoft.Extensions.Options;
using TXC.Common.Data.TenantDbConnection;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace Services.Queries.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetMediaQueryHandler : ServiceHandlerBase ,IRequestHandlerWrapper<GetMediaQuery, MediaDto>
    {
        private IDbConnection _dbConnection;
        private readonly CdnConfiguration _cdnConfig;
        private readonly int _tenantId;
        private readonly string _TX2UserName;
        private readonly string _tenantName;
        public GetMediaQueryHandler(ITenantDbConnection tenantDbConnection
                                   ,IOptions<CdnConfiguration> cdnConfig
                                   ,IDapperOperation dapperOperation
                                   ,IHttpContextAccessor httpContextAccessor
                                   ) : base (tenantDbConnection , dapperOperation)
        {
            _cdnConfig = cdnConfig.Value;
            _tenantName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantName];
            _tenantId = int.Parse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId]);
            _TX2UserName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TX2UserName];
        }

        public async Task<Response<MediaDto>> Handle(GetMediaQuery request, CancellationToken cancellationToken)
        {

            // initialize db connection
            var conn = await _tenantDbConnection.GetTenantDbConnection(_tenantId.ToString(),false, cancellationToken);

            if (!conn.Success)
                return Response.Fail<MediaDto>("Error in Tenant DB", null);

            _dbConnection = conn.Data;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@MediaId", request.MediaId, DbType.String, ParameterDirection.Input);

            CommandDefinition commandDefinition = new CommandDefinition(SqlSelectMedia(), commandType: CommandType.Text,
                                                                        parameters: parameters, cancellationToken: cancellationToken);

            var result = await _dapperOperation.ProcessSql<SelectSingle<MediaDto>, MediaDto>(_dbConnection, commandDefinition);

            if (result != null)
            {
                result.NodeUrl = $"{_cdnConfig.ImageCdnUri}{result.NodeUrl}";
            }

            return Response.Success("sucess", result);
        }

        private string SqlSelectMedia()
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
	                FROM media.tb_m_media WHERE media_id = @MediaId	
            ";
        }
    }
}

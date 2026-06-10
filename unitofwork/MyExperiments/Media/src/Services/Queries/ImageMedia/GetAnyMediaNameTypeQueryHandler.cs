using Dapper;
using TXC.Common.Data.TenantDbConnection;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;
using Microsoft.Extensions.Logging;
using Services.Command.ImageMedia;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace Services.Queries.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetAnyMediaNameTypeQueryHandler : ServiceHandlerBase ,IRequestHandlerWrapper<GetAnyMediaNameTypeQuery, bool>
    {
        private readonly int _tenantId;
        private readonly string _TX2UserName;
        private readonly string _tenantName;
        private IDbConnection _dbConnection;
        public GetAnyMediaNameTypeQueryHandler(ITenantDbConnection tenantDbConnection, IDapperOperation dapperOperation, IHttpContextAccessor httpContextAccessor) : base (tenantDbConnection,dapperOperation)
        {
            _tenantName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantName];
            _tenantId = int.Parse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId]);
            _TX2UserName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TX2UserName];
        }
        public async Task<Response<bool>> Handle(GetAnyMediaNameTypeQuery request, CancellationToken cancellationToken)        {
            var conn = await _tenantDbConnection.GetTenantDbConnection(_tenantId.ToString(),false, cancellationToken);

            if (!conn.Success)
                return Response.Fail("Error in Tenant DB", false);

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Keyword", request.Keyword, DbType.String, ParameterDirection.Input);
            parameters.Add("@Type", request.Type, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@IsHave", false, DbType.Boolean, ParameterDirection.Output);

            CommandDefinition commandDefinition = new CommandDefinition(SqlGetMediaByNametype(), commandType: CommandType.Text,
                                                        parameters: parameters, cancellationToken: cancellationToken);

            await _dapperOperation.ProcessSql<ExecuteCommand, int>(conn.Data, commandDefinition);

            var result = parameters.Get<bool>("@IsHave");

            return Response.Success<bool>("Success",result);
        }

        private string SqlGetMediaByNametype(){
            return @"
             	IF EXISTS (	SELECT * FROM media.tb_m_media where keyword = @Keyword AND [type] = @Type)
	        	    BEGIN
	        	    	SET @IsHave = 1
	        	    END
	            ELSE
	        	    BEGIN
	        	    	SET @IsHave = 0
	        	    END
            ";
        }
    }
}

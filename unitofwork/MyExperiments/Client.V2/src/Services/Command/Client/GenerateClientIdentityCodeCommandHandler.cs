using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Domain;
using TXC.Common.Services.Wrappers;

namespace Services.Command.Client
{
    public class GenerateClientIdentityCodeCommandHandler : IRequestHandlerWrapper<GenerateClientIdentityCodeCommand, string>
    {
        private readonly ITenantDbConnection _tenantDbConnection;
        private readonly IDapperOperation _dapperOperation;
        private IDbConnection _dbConnection;

        public GenerateClientIdentityCodeCommandHandler(ITenantDbConnection tenantDbConnection, IDapperOperation dapperOperation)
        {
            _dapperOperation = dapperOperation;
            _tenantDbConnection = tenantDbConnection;
        }

        public async Task<Response<string>> Handle(GenerateClientIdentityCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {

                // initialize db connection
                var conn = await _tenantDbConnection.GetTenantDbConnection(request.TenantID.ToString(), false, default);

                if (!conn.Success)
                    return Response.Fail("Error in Tenant DB", default(string));

                _dbConnection = conn.Data;

                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@SequenceName", request.SequenceName, DbType.AnsiString, ParameterDirection.Input);
                parameters.Add("@IsFixReturnLength", request.IsFixReturnLength, DbType.Boolean, ParameterDirection.Input);
                parameters.Add("@ReturnLength", request.ReturnLength, DbType.Int16, ParameterDirection.Input);
                parameters.Add("@PaddingCharacter", request.PaddingCharacter, DbType.AnsiString, ParameterDirection.Input);
                parameters.Add("@ret", string.Empty, DbType.AnsiString, ParameterDirection.Output);

                CommandDefinition commandDefinition = new CommandDefinition("client.sp_generate_identity_code", commandType: CommandType.StoredProcedure,
                                                                        parameters: parameters, cancellationToken: cancellationToken);

                var dbResult = await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<string>, string>(_dbConnection, commandDefinition);

                if (string.IsNullOrWhiteSpace(dbResult))
                {
                    return Response.Fail("Failed to generate client identity code", default(string));
                }

                return Response.Success("Success", dbResult);
            }
            catch (Exception exception)
            {
                return Response.Fail(exception.Message, default(string));
            }
        }
    }
}

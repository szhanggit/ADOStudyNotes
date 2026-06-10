using Dapper;
using DapperTXC.Core;
using DapperTXC.Infrastructure;
using DapperTXC.Operations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DapperTXC
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDbConnection _dbConnection;
        protected readonly ITenantDbConnection _tenantDbConnection;
        protected readonly IDapperOperation _dapperOperation;
        public EmployeeRepository(ITenantDbConnection tenantDbConnection, IDapperOperation dapperOperation)
        {
            _tenantDbConnection = tenantDbConnection;
            _dapperOperation = dapperOperation;
        }

        public async Task<Response<int>> AddEmployee()
        {
            CancellationToken cancellationToken = new CancellationToken();
            var conn = await _tenantDbConnection.GetTenantDbConnection(false, cancellationToken);

            if (!conn.Success)
            {
                return Response.Fail(conn.Message, 0);
            }
            _dbConnection = conn.Data;
            
            int Id = 3;
            string Name = "Peter";
            string Gender = "Male";
            DateTime DateOfBirth = new DateTime(1990, 1, 1);
            int EmployeeType = 2;
            int AnnualSalary = 0;
            int HourlyPay = 30;
            int HoursWorked = 40;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Name", Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Gender", Gender, DbType.String, ParameterDirection.Input);
            parameters.Add("@DateOfBirth", DateOfBirth, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@EmployeeType", EmployeeType, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@AnnualSalary", AnnualSalary, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@HourlyPay", HourlyPay, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@HoursWorked", HoursWorked, DbType.Int32, ParameterDirection.Input);
            CommandDefinition commandDefinition = new CommandDefinition("spSaveEmployee2", commandType: CommandType.StoredProcedure, parameters: parameters, cancellationToken: cancellationToken);

            var dbResult = await _dapperOperation.ProcessSql<ExecuteCommand, int>(_dbConnection, commandDefinition);
            return Response.Success("Success", 1);
        }
    }
}

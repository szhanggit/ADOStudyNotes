using Dapper;
using DapperTxcApi.Core;
using DapperTxcApi.Infrastructure;
using DapperTxcApi.Models;
using DapperTxcApi.Operations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DapperTxcApi
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

        public async Task<Response<IEnumerable<EmployItem>>> AllEmployee()
        {
            var conn = await _tenantDbConnection.GetTenantDbConnection(false, default);
            _dbConnection = conn.Data;

            CommandDefinition commandDefinition = new CommandDefinition("spGetAllEmployees", commandType: CommandType.StoredProcedure,
                                                                        parameters: null);

            IEnumerable<EmployItem> result = await _dapperOperation.ProcessSql<SelectMany<EmployItem>, IEnumerable<EmployItem>>(_dbConnection, commandDefinition);
            return Response.Success("Success", result);
        }

        public async Task<Response<EmployItem>> GetEmployeeById(int EmployeeId)
        {
            var conn = await _tenantDbConnection.GetTenantDbConnection(false, default);
            _dbConnection = conn.Data;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", EmployeeId, DbType.Int32, ParameterDirection.Input);

            CommandDefinition commandDefinition = new CommandDefinition("spGetEmployee2", commandType: CommandType.StoredProcedure,
                                                                        parameters: parameters);
            EmployItem _employeeInfo = await _dapperOperation.ProcessSql<ExecuteCommandWithReturn<EmployItem>, EmployItem>(_dbConnection, commandDefinition);
            return Response.Success("Success", _employeeInfo);
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

        public async Task<Response<int>> UpdateEmployeeInfoById()
        {
            var conn = await _tenantDbConnection.GetTenantDbConnection(false, default);

            if (!conn.Success)
            {
                return Response.Fail(conn.Message, 0);
            }
            _dbConnection = conn.Data;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", 1, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Name", "update", DbType.String, ParameterDirection.Input);
            parameters.Add("@Gender", "Female", DbType.String, ParameterDirection.Input);
            parameters.Add("@City", "London", DbType.String, ParameterDirection.Input);
            parameters.Add("@DateOfBirth", new DateTime(1979, 1, 5), DbType.DateTime, ParameterDirection.Input);
            CommandDefinition commandDefinition = new CommandDefinition("spSaveEmployee_Update", commandType: CommandType.StoredProcedure, parameters: parameters, cancellationToken: default);

            var dbResult = await _dapperOperation.ProcessSql<ExecuteCommand, int>(_dbConnection, commandDefinition);
            return Response.Success("Success", 1);
        }
    }
}

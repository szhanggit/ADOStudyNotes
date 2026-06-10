using DapperTxcApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTxcApi
{
    public interface IEmployeeRepository
    {
        Task<Response<IEnumerable<EmployItem>>> AllEmployee();
        Task<Response<EmployItem>> GetEmployeeById(int EmployeeId);
        Task<Response<int>> AddEmployee();
        Task<Response<int>> UpdateEmployeeInfoById();
    }
}

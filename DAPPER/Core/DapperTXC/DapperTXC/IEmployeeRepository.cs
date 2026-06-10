using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTXC
{
    public interface IEmployeeRepository
    {
        Task<Response<int>> AddEmployee();
    }
}

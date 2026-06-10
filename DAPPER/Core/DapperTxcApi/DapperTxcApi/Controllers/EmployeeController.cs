using DapperTxcApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DapperTxcApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeRepository _employeeRepository = null;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Route("AllEmployees")]
        public async Task<IActionResult> GetAll([FromQuery] int EmployeeNum, CancellationToken cancellationToken)
        {
            var result = await _employeeRepository.AllEmployee();
            IEnumerable<EmployItem> _employeeList = result.Data;
            return Ok(new ResponseMessageDto
            {
                Message = "Success",
                Code = 200
            });
        }

        [HttpGet]
        [Route("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById([FromQuery] int EmployeeNum, CancellationToken cancellationToken)
        {
            var result = await _employeeRepository.GetEmployeeById(1);
            EmployItem _employeeInfo = result.Data;
            return Ok(new ResponseMessageDto
            {
                Message = "Success",
                Code = 200
            });
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> Create([FromForm] int EmployeeNum, CancellationToken cancellationToken)
        {
            await _employeeRepository.AddEmployee();
            return Ok(new ResponseMessageDto
            {
                Message = "Success",
                Code = 200
            });
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Put([FromForm] int EmployeeNum, CancellationToken cancellationToken)
        {
            await _employeeRepository.UpdateEmployeeInfoById();
            return Ok(new ResponseMessageDto
            {
                Message = "Success",
                Code = 200
            });
        }
    }
}

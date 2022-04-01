using dvt_jibble_backend.Enums;
using dvt_jibble_backend.Models;
using dvt_jibble_backend.Models.Dtos;
using dvt_jibble_backend.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace dvt_jibble_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_developmentOrigins")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet(Name = "GetEmployee")]
        public async Task<EmployeeDto> Get([FromQuery] PagingDto page)
        {
            try
            {
                var numberOfEmployees = await _employeeService.GetEmployeeCount();
                var employees = await _employeeService.GetEmployees(page);
                return new EmployeeDto
                {
                    Status = APIStatus.Success,
                    NumberOfEmployees = numberOfEmployees,
                    Employees = employees,
                };
            }
            catch (Exception ex)
            {
                return new EmployeeDto
                {
                    Status = APIStatus.Failed,
                    ErrorMessage = ex.Message,
                };
            }

        }

        [HttpPost]
        [Route("ImportEmployees")]
        [RequestSizeLimit(200_000_000)]
        public async Task<EmployeeDto> ImportEmployees(List<Employee> employees)
        {
            try
            {
                await _employeeService.ImportEmployees(employees);
                return new EmployeeDto
                {
                    Status = APIStatus.Success,
                };
            }
            catch (Exception ex)
            {
                return new EmployeeDto
                {
                    Status = APIStatus.Failed,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPost]
        [Route("UpdateEmployees")]
        public async Task<EmployeeDto> UpdateEmployees(List<Employee> employees)
        {
            try
            {
                await _employeeService.UpdateEmployees(employees);
                return new EmployeeDto
                {
                    Status = APIStatus.Success,
                };
            }
            catch (Exception ex)
            {
                return new EmployeeDto
                {
                    Status = APIStatus.Failed,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}

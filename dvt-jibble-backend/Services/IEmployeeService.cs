using dvt_jibble_backend.Models;
using dvt_jibble_backend.Models.Dtos;

namespace dvt_jibble_backend.Services
{
    public interface IEmployeeService
    {
        Task<int> GetEmployeeCount();
        Task<List<Employee>> GetEmployees(PagingDto page);
        Task ImportEmployees(List<Employee> employees);
        Task UpdateEmployees(List<Employee> employees);
    }
}

using dvt_jibble_backend.DbContexts;
using dvt_jibble_backend.Dependencies;
using dvt_jibble_backend.Models;
using dvt_jibble_backend.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace dvt_jibble_backend.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeContext _context;
        private readonly IDatabaseDependency _npgsqlDependency;
        public EmployeeService(EmployeeContext context, IDatabaseDependency npgsqlDependency)
        {
            _context = context;
            _npgsqlDependency = npgsqlDependency;
        }
        public Task<int> GetEmployeeCount()
        {
            return _context.Employees.CountAsync();
        }
        public Task<List<Employee>> GetEmployees(PagingDto page)
        {
            return _context.Employees.OrderBy(o => o.CreatedAt).ThenBy(o => o.Id)
                    .Skip(page.Start).Take(page.ItemPerPage).ToListAsync();
        }
        public Task ImportEmployees(List<Employee> employees)
        {
            return _npgsqlDependency.Copy(employees);
        }
        public Task UpdateEmployees(List<Employee> employees)
        {
            _context.Employees.UpdateRange(employees);
            return _context.SaveChangesAsync();
        }
    }
}

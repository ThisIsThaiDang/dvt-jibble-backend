using dvt_jibble_backend.DbContexts;
using dvt_jibble_backend.Dependencies;
using dvt_jibble_backend.Models;
using dvt_jibble_backend.Models.Dtos;
using dvt_jibble_backend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace dvt_jibble_backend_tests
{
    [Collection("Sequential")]
    public class EmployeeServiceTests : IDisposable
    {
        EmployeeContext _context;
        IEmployeeService _employeeService;
        public EmployeeServiceTests()
        {
            NpgsqlDependency _npgsqlDependency = new NpgsqlDependency(DummyData.testingConnection);
            _context = new EmployeeContext(
                new DbContextOptionsBuilder<EmployeeContext>().UseNpgsql(DummyData.testingConnection).Options
            );
            _employeeService = new EmployeeService(_context, _npgsqlDependency);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetEmployeeCount_1000()
        {
            // Arrange
            List<Employee> employees = DummyData.Employees_1K();
            await _employeeService.ImportEmployees(employees);
            // Act
            var employeeCount = await _employeeService.GetEmployeeCount();
            // Assert
            Assert.Equal(1000, employeeCount);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task GetEmployees_1_100()
        {
            // Arrange
            List<Employee> employees = DummyData.Employees_1K();
            PagingDto page = DummyData.Page_1_100();
            await _employeeService.ImportEmployees(employees);
            // Act
            List<Employee> dbEmployees = await _employeeService.GetEmployees(page);
            // Assert
            Employee employee = dbEmployees[0];
            Assert.NotNull(employee);
            Assert.Equal($"{DummyData.PREFIX_EMP_ID}{1}", employee.EmpId);
            Assert.Equal(100, dbEmployees.Count);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task GetEmployees_1_500()
        {
            // Arrange
            List<Employee> employees = DummyData.Employees_1K();
            PagingDto page = DummyData.Page_5_100();
            await _employeeService.ImportEmployees(employees);
            // Act
            List<Employee> dbEmployees = await _employeeService.GetEmployees(page);
            // Assert
            Employee employee = dbEmployees[0];
            Assert.NotNull(employee);
            Assert.Equal($"{DummyData.PREFIX_EMP_ID}{401}", employee.EmpId);
            Assert.Equal(100, dbEmployees.Count);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task UpdateEmployees()
        {
            // Arrange
            List<Employee> employees = DummyData.Employees_1K();
            PagingDto page = DummyData.Page_5_100();
            await _employeeService.ImportEmployees(employees);
            List<Employee> dbEmployees = await _employeeService.GetEmployees(page);
            List<Employee> updateEmployees = new List<Employee>();
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            Employee employee1 = dbEmployees[0];
            employee1.EmpId = "CHANGE_EmpId_1";
            employee1.FirstName = "CHANGE_FirstName_1";
            employee1.LastName = "CHANGE_LastName_1";
            employee1.DateOfBirth = now;
            updateEmployees.Add(employee1);
            Employee employee2 = dbEmployees[1];
            employee2.EmpId = null;
            employee2.FirstName = null;
            employee2.LastName = null;
            employee2.DateOfBirth = null;
            updateEmployees.Add(employee2);
            // Act
            await _employeeService.UpdateEmployees(updateEmployees);
            // Assert
            dbEmployees = await _employeeService.GetEmployees(page);
            employee1 = dbEmployees[0];
            Assert.Equal("CHANGE_EmpId_1", employee1.EmpId);
            Assert.Equal("CHANGE_FirstName_1", employee1.FirstName);
            Assert.Equal("CHANGE_LastName_1", employee1.LastName);
            Assert.Equal(now, employee1.DateOfBirth);
            employee2 = dbEmployees[1];
            Assert.Null(employee2.EmpId);
            Assert.Null(employee2.FirstName);
            Assert.Null(employee2.LastName);
            Assert.Null(employee2.DateOfBirth);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
    }
}

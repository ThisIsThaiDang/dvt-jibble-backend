using dvt_jibble_backend.Controllers;
using dvt_jibble_backend.DbContexts;
using dvt_jibble_backend.Dependencies;
using dvt_jibble_backend.Enums;
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
    [CollectionDefinition(name: "Sequential", DisableParallelization = true)]
    public class EmployeeControllerTests : IDisposable
    {
        EmployeeController _employeeController;
        EmployeeContext _context;
        IEmployeeService _employeeService;

        public EmployeeControllerTests()
        {
            _context = new EmployeeContext(
                new DbContextOptionsBuilder<EmployeeContext>().UseNpgsql(DummyData.testingConnection).Options
            );
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var _npgsqlDependency = new NpgsqlDependency(DummyData.testingConnection);
            _employeeService = new EmployeeService(_context, _npgsqlDependency);
            _employeeController = new EmployeeController(_employeeService);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetEmployee_Success()
        {
            // Arrange
            List<Employee> employees = DummyData.Employees_1K();
            PagingDto page = DummyData.Page_1_100();
            await _employeeService.ImportEmployees(employees);
            // Act
            var employeeDto = await _employeeController.Get(page);
            // Assert
            Assert.NotNull(employeeDto);
            Assert.IsType<EmployeeDto>(employeeDto);
            Assert.Equal(APIStatus.Success, employeeDto.Status);
            Assert.Equal(1000, employeeDto.NumberOfEmployees);
            Assert.Equal(100, employeeDto.Employees.Count);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task GetEmployee_Failed()
        {
            // Arrange
            List<Employee> employees = DummyData.Employees_1K();
            PagingDto page = DummyData.Page_Negative_100();
            await _employeeService.ImportEmployees(employees);
            // Act
            var employeeDto = await _employeeController.Get(page);
            // Assert
            Assert.NotNull(employeeDto);
            Assert.IsType<EmployeeDto>(employeeDto);
            Assert.Equal(APIStatus.Failed, employeeDto.Status);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task ImportEmployees_Success()
        {
            // Arrange
            List<Employee> employees = DummyData.Employees_1K();
            PagingDto page = DummyData.Page_1_100();
            // Act
            var employeeDto = await _employeeController.ImportEmployees(employees);
            // Assert
            Assert.NotNull(employeeDto);
            Assert.IsType<EmployeeDto>(employeeDto);
            Assert.Equal(APIStatus.Success, employeeDto.Status);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task ImportEmployees_Failed()
        {
            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var employeeDto = await _employeeController.ImportEmployees(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            // Assert
            Assert.NotNull(employeeDto);
            Assert.IsType<EmployeeDto>(employeeDto);
            Assert.Equal(APIStatus.Failed, employeeDto.Status);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task UpdateEmployees_Success()
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
            var employeeDto = await _employeeController.UpdateEmployees(updateEmployees);
            // Assert
            Assert.NotNull(employeeDto);
            Assert.IsType<EmployeeDto>(employeeDto);
            Assert.Equal(APIStatus.Success, employeeDto.Status);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task UpdateEmployees_Failed()
        {
            // Arrange
            List<Employee> employees = DummyData.Employees_1K();
            await _employeeService.ImportEmployees(employees);
            List<Employee> updateEmployees = DummyData.Employees_10_all_null();
            // Act
            var employeeDto = await _employeeController.UpdateEmployees(updateEmployees);
            // Assert
            Assert.NotNull(employeeDto);
            Assert.IsType<EmployeeDto>(employeeDto);
            Assert.Equal(APIStatus.Failed, employeeDto.Status);
            // Clear
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }

    }
}
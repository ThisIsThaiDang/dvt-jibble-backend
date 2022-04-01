using dvt_jibble_backend.DbContexts;
using dvt_jibble_backend.Dependencies;
using dvt_jibble_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace dvt_jibble_backend_tests
{
    [Collection("Sequential")]
    public class NpgsqlDependencyTests : IDisposable
    {
        IDatabaseDependency _npgsqlDependency;
        EmployeeContext _context;
        public NpgsqlDependencyTests() {
            _npgsqlDependency = new NpgsqlDependency(DummyData.testingConnection);
            _context = new EmployeeContext(
                new DbContextOptionsBuilder<EmployeeContext>().UseNpgsql(DummyData.testingConnection).Options
            );
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
        [Fact]
        public async Task Copy_1K()
        {
            List<Employee> employees = DummyData.Employees_1K();
            await _npgsqlDependency.Copy(employees);
            var employeeCount = await _context.Employees.CountAsync();
            Assert.Equal(1000, employeeCount);
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task Copy_1M()
        {
            List<Employee> employees = DummyData.Employees_1M();
            await _npgsqlDependency.Copy(employees);
            var employeeCount = await _context.Employees.CountAsync();
            Assert.Equal(1000000, employeeCount);
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
        [Fact]
        public async Task Copy_10_all_null()
        {
            List<Employee> employees = DummyData.Employees_10_all_null();
            await _npgsqlDependency.Copy(employees);
            var employeeCount = await _context.Employees.CountAsync();
            Assert.Equal(10, employeeCount);
            _context.Database.ExecuteSqlRaw("TRUNCATE employee");
        }
    }
}

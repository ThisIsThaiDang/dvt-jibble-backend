using dvt_jibble_backend.Models;
using dvt_jibble_backend.Models.Dtos;
using System;
using System.Collections.Generic;

namespace dvt_jibble_backend_tests
{
    public static class DummyData
    {
        public static string testingConnection = "Server=localhost;Database=jibble_employee_tests;Port=5432;User Id=postgres;Password=postgres;Timeout=1024;Command Timeout=1024;Pooling=false";
        public static string PREFIX_EMP_ID = "EMPLOYEE_";
        public static string PREFIX_FIRST_NAME = "FirstName_";
        public static string PREFIX_LAST_NAME = "LastName_";

        public static PagingDto Page_Negative_100()
        {
            return new PagingDto()
            {
                CurrentPage = -1,
                ItemPerPage = 100,
            };
        }

        public static PagingDto Page_1_100()
        {
            return new PagingDto()
            {
                CurrentPage = 1,
                ItemPerPage = 100,
            };
        }

        public static PagingDto Page_5_100()
        {
            return new PagingDto()
            {
                CurrentPage = 5,
                ItemPerPage = 100,
            };
        }

        public static List<Employee> Employees_10_all_null()
        {
            List<Employee> employees = new List<Employee>();
            for (int i = 0; i < 10; i++)
            {
                Employee employee = new Employee()
                {
                    EmpId = null,
                    FirstName = null,
                    LastName = null,
                    DateOfBirth = null,
                };
                employees.Add(employee);
            }
            return employees;
        }

        public static List<Employee> Employees_1K()
        {
            List<Employee> employees = new List<Employee>();
            for (int i = 1; i <= 1000; i++)
            {
                Employee employee = new Employee()
                {
                    EmpId = $"{PREFIX_EMP_ID}{1001 - i}",
                    FirstName = $"{PREFIX_FIRST_NAME}{1001 - i}",
                    LastName = $"{PREFIX_LAST_NAME}{1001 - i}",
                    DateOfBirth = System.DateTime.Now,
                    CreatedAt = System.DateTime.Now.AddMinutes(-i)
                };
                employees.Add(employee);
            }
            return employees;
        }

        public static List<Employee> Employees_1M()
        {
            List<Employee> employees = new List<Employee>();
            for (int i = 0; i < 1000000; i++)
            {
                Employee employee = new Employee()
                {
                    EmpId = $"EMPLOYEE_{i}",
                    FirstName = $"FirstName_{i}",
                    LastName = $"LastName_{i}",
                    DateOfBirth = System.DateTime.Now,
                };
                employees.Add(employee);
            }
            return employees;
        }
    }
}

using dvt_jibble_backend.Enums;

namespace dvt_jibble_backend.Models.Dtos
{
    public class EmployeeDto
    {
        public APIStatus Status { get; set; }
        public string ErrorMessage { get; set; } = String.Empty;
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public int NumberOfEmployees { get; set; } = 0;
    }
}

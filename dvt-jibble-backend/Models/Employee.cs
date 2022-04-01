using System.ComponentModel.DataAnnotations.Schema;

namespace dvt_jibble_backend.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        [Column("emp_id")]
        public string? EmpId { get; set; }
        [Column("first_name")]
        public string? FirstName { get; set; }
        [Column("last_name")]
        public string? LastName { get; set; }
        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

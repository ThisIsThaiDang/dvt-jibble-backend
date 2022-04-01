using dvt_jibble_backend.Models;

namespace dvt_jibble_backend.Dependencies
{
    public interface IDatabaseDependency
    {
        Task Copy(List<Employee> dtos);
    }
}

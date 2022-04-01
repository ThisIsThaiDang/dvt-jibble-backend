using dvt_jibble_backend.Models;
using Npgsql;

namespace dvt_jibble_backend.Dependencies
{
    public class NpgsqlDependency : IDatabaseDependency
    {
        NpgsqlConnection _connection;
        public NpgsqlDependency(string strConnection)
        {
            _connection = new NpgsqlConnection(strConnection);
        }
        public async Task Copy(List<Employee> dtos)
        {
            try
            {
                string query = $@"copy Employee (
                                emp_id,
                                first_name,
	                            last_name,
	                            date_of_birth,
                                created_at
                             )
                           from STDIN BINARY";
                _connection.Open();
                using var writer = _connection.BeginBinaryImport(query);
                foreach (Employee dto in dtos)
                {
                    writer.StartRow();
                    if (dto.EmpId == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.Write(dto.EmpId);
                    }
                    if (dto.FirstName == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.Write(dto.FirstName);
                    }
                    if (dto.LastName == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.Write(dto.LastName);
                    }
                    if (dto.DateOfBirth == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.Write((DateTime)dto.DateOfBirth);
                    }
                    writer.Write(dto.CreatedAt);
                }
                await writer.CompleteAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}

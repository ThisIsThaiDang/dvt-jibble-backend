using dvt_jibble_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace dvt_jibble_backend.DbContexts
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");
                entity.Property(x => x.Id)
                    .HasColumnName("id")
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .IsRequired();
            });
        }
    }
}

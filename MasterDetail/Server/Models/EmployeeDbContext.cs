using MasterDetail.Shared;
using Microsoft.EntityFrameworkCore;

namespace MasterDetail.Server.Models
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) {}

        public DbSet<Employee>  Employees { get; set; } = default!;
        public DbSet<Department>  Departments { get; set; } = default!;
        public DbSet<BookingEntry> BookingEntries { get; set; } = default!;
    }
}

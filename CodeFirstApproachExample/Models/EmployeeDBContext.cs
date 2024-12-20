using Microsoft.EntityFrameworkCore;

namespace CodeFirstApproachExample.Models
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions options) : base(options)
        { 


        
        }
        public DbSet<Employee> Employees { get; set;}


    }
}

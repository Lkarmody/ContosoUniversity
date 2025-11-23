using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class InstructorContext : DbContext
    {
        public InstructorContext (DbContextOptions<InstructorContext> options)
            : base(options)
        {
        }
        public DbSet<Instructor> Instructors { get; set; } = default!;
    
    }
}

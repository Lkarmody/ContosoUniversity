using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext (DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
        }

      
        #region snippet1
        public async virtual Task<List<Student>> GetStudentsAsync()
        {
            return await Students
                .OrderBy(Student => Student.LastName)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region snippet2
        public async virtual Task AddStudentAsync(Student Student)
        {
            await Students.AddAsync(Student);
            await SaveChangesAsync();
        }
        #endregion

        public static List<Student> GetSeedingMessages()
        {
            return new List<Student>()
            {
                new Student(){ LastName = "You're standing on my scarf." },
                new Student(){ LastName = "Would you like a jelly baby?" },
                new Student(){ LastName = "To the rational mind, nothing is inexplicable; only unexplained." }
            };
          }
        }
}

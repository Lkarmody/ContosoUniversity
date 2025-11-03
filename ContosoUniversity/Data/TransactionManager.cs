using System;
using System.Collections.Generic;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;

namespace ContosoUniversity.Data
{
    public class TransactionManager
    {
        private readonly SchoolContext _context;

            public TransactionManager(SchoolContext context)
            {   
                _context = context;
            
            }   
           
            public async Task HandleMultiEntityTransactionAsync()
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var student = new Student
                    {
                        LastName = "Doe",
                        FirstMidName = "Jane",
                        EnrollmentDate = DateTime.Now
                    };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();


                    var enrollment = new Enrollment
                    {
                        StudentID = student.ID,
                        CourseID = 1050,
                        Grade = Grade.A
                    };

                    _context.Enrollments.Add(enrollment);
                    await _context.SaveChangesAsync();

                   await transaction.CommitAsync();


            }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
        }
    }
}

using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ContosoUniversity.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly SchoolContext _context;

        public string? DefaultLogLevel { get; set; }
        public string? EnvironmentMessage { get; set; }
        public string? ConcurrencyMessage { get; set; }
        
        public string? StoredProcFirstName { get; set; }
        public string? StoredProcLastName { get; set; }
        public string? StoredProcEnrollmentDate { get; set; }
        public SettingsModel(IConfiguration configuration, SchoolContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        public async Task OnGetAsync()
        {
            DefaultLogLevel = _configuration["Logging:LogLevel:Default"];
            EnvironmentMessage = _configuration["CustomSettings:EnvironmentMessage"];

            try
            {
                var student = await _context.Students.FirstAsync();

                student.EnrollmentDate = student.EnrollmentDate.AddDays(1);

                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Student SET FirstMidName = 'Conflict' WHERE ID = {0}", student.ID);

                await _context.SaveChangesAsync();

                ConcurrencyMessage = "Changes saved successfully.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var databaseValues = await entry.GetDatabaseValuesAsync();

                entry.OriginalValues.SetValues(databaseValues);

                await _context.SaveChangesAsync();

                ConcurrencyMessage = "Concurrency conflict detected and resolved.";
            }

            var studentlastNameParam = new SqlParameter("@StudentLastName", "Doe");

            var result = await _context.Students
                .FromSqlRaw("EXEC DisplayStudentFromLastName @StudentLastName", studentlastNameParam)
                .ToListAsync();

            if (result.Count > 0)
            {
                StoredProcFirstName = result[0].FirstMidName;
                StoredProcLastName = result[0].LastName;
                StoredProcEnrollmentDate = result[0].EnrollmentDate.ToShortDateString();
            }
            else
            {
                StoredProcFirstName = "No matching student found";
                StoredProcLastName = null;
                StoredProcEnrollmentDate = null;
            }
        }

        public class StudentSPResult
        {
            public string FirstMidName { get; set; }
            public DateTime EnrollmentDate { get; set; }
            
        }
    }
    }

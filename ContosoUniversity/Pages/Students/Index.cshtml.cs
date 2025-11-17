using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {


        private readonly ContosoUniversity.Data.SchoolContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ContosoUniversity.Data.SchoolContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Student> Student { get;set; } = default!;

        public async Task OnGetAsync()
        {

            Student = await _context.Students.ToListAsync();

            
            Student = await _context.GetStudentsAsync();

            int count = await _context.Students.CountAsync();

            // Structured log
            _logger.LogInformation(
            "Accessed {Page} at {Time} with StudentCount {Count}",
            "Students Index",
            DateTime.Now,
            count);
        }
        public async Task<IActionResult> OnPostAddStudentAsync()
        {
            if (!ModelState.IsValid)
            {
                Student = await _context.GetStudentsAsync();

                return Page();
            }

            await _context.AddStudentAsync((Student)Student);

            return RedirectToPage();
        }
    }
}

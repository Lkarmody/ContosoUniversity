using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;

namespace ContosoUniversity.Data
{
    public class InstructorEndpoints
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/Test", async context =>
            {
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "Test Passed" });
                });
                app.MapGet("/Instructors", (InstructorContext db) =>
                db.Instructors.ToListAsync());
                app.MapGet("/Instructors/Retired", (InstructorContext db) =>
                db.Instructors.Where(t => t.IsRetired).ToListAsync());
                app.MapGet("/Instructors/{id}", (int id, InstructorContext db) =>
                db.Instructors.FindAsync(id));
                app.MapPost("/Instructors", (Instructor instructor, InstructorContext
                db) =>
                {
                    db.Instructors.Add(instructor);
                    db.SaveChanges();
                    return Results.Created($"/Instructors/{instructor.Id}",
                    instructor);
                });
                app.MapPut("/Instructors/{id}", async (int id, Instructor
                instructorInput, InstructorContext db) =>
                {
                    var instructor = db.Instructors.Find(id);
                    if (instructor == null) return Results.NotFound();
                    instructor.Name = instructorInput.Name;
                    instructor.IsRetired = instructorInput.IsRetired;
                    db.SaveChanges();
                    return Results.NoContent();
                });
                app.MapDelete("Instructors/{id}", (int id, InstructorContext db) =>
                {
                    if (db.Instructors.Find(id) is Instructor instructor)
                    {
                        db.Instructors.Remove(instructor);
                        db.SaveChanges();
                        return Results.NoContent();
                    }
                    return Results.NotFound();
                });
           }
    }
}

    
    

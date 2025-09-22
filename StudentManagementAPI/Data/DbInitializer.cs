using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // εφαρμογή migrations
            context.Database.Migrate();

            // Αν υπάρχουν courses δεν κάνει seeding
            if (context.Courses.Any())
                return; 

            //Αρχικά courses
            var courses = new Course[]
            {
                new Course { Name = "Computer Science", Description = "Basics of C#" },
                new Course { Name = "Mathematics", Description = "Geografia" }
            };
            context.Courses.AddRange(courses);
            context.SaveChanges();
            //Αρχικοί student
            var students = new Student[]
            {
                new Student { FirstName = "George", LastName = "Fotopoulos", Email = "george@test.com", Age = 42, Course = courses[0] },
                new Student { FirstName = "Maria", LastName = "Papadopoulou", Email = "maria@test.com", Age = 25, Course = courses[1] }
            };
            context.Students.AddRange(students);
            context.SaveChanges();
        }
    }
}

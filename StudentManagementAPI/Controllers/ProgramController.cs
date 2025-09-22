using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers
{
    // Controller για Courses με CRUD και διαχείριση Students
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/courses
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCourses()
        {
            var courses = _context.Courses
                .Include(c => c.Students)
                .AsEnumerable()  //Μεταφέρά δεδομένων στη μνήμη
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Students = c.Students.Select(s => new StudentDto
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        Age = s.Age,
                        CourseId = s.CourseId,
                        CourseName = c.Name
                    }).ToList()
                })
                .ToList();

            return Ok(courses);
        }

        // GET: api/courses/
        [HttpGet("{id}")]
        public ActionResult<CourseDto> GetCourse(int id)
        {
            var course = _context.Courses
                .Include(c => c.Students)
                .AsEnumerable()  // << Εδώ επίσης
                .Where(c => c.Id == id)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Students = c.Students.Select(s => new StudentDto
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        Age = s.Age,
                        CourseId = s.CourseId,
                        CourseName = c.Name
                    }).ToList()
                })
                .FirstOrDefault();

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto dto)
        {
            // Ψάχνει αν υπάρχει ήδη course με το ίδιο όνομα
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Name == dto.Name);

            if (course == null)
            {
                // Δημιουργούμε νέο course αν δεν υπάρχει
                course = new Course
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Students = new List<Student>()
                };
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
            }

            // Προσθέτει τους students στο υπάρχον course
            if (dto.Students != null)
            {
                foreach (var s in dto.Students)
                {
                    var student = new Student
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        Age = s.Age,
                        CourseId = course.Id
                    };
                    _context.Students.Add(student);
                }
                await _context.SaveChangesAsync();
            }

            // Δημιουργεί DTO για επιστροφή
            var result = new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Students = course.Students.Select(s => new StudentDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Age = s.Age,
                    CourseId = course.Id,
                    CourseName = course.Name
                }).ToList()
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, result);
        }

        // PUT: api/students/
        [HttpPut("student/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.Age = dto.Age;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/course/
        [HttpPut("courses/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDto dto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            course.Name = dto.Name;
            course.Description = dto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/courses/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/students/
        [HttpDelete("students/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

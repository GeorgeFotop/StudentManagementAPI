using System.Collections.Generic;

namespace StudentManagementAPI.DTOs
{
    public class CreateCourseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<CreateStudentDto>? Students { get; set; }
    }
}
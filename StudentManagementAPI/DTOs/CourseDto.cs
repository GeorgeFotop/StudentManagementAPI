using StudentManagementAPI.DTOs;

namespace StudentManagementAPI.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
    }
}

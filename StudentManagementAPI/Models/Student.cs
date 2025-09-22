namespace StudentManagementAPI.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }

        
        public int CourseId { get; set; }

        
        public Course? Course { get; set; }
    }

}

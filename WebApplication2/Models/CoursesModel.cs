namespace WebApplication2.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string CourseNumber { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string ? Faculty { get; set; }

        public int CourseCreditHours { get; set; }

        public string? CourseDescription { get; set; }

        public string? CourseCategory { get; set; }

        public string TeacherName { get; set; } = null!;

        public DateTime? CourseStartDate { get; set; }

        public DateTime? CourseEndDate { get; set; }

     
    }
}

namespace WebApplication2.Models
{
    public class Grades
    {
        public int Id { get; set; }

        public string? CourseName { get; set; }
        public string CourseNumber { get; set; } = null!;
        public string? StudentName { get; set; }
        public string StudentNumber { get; set; } = null!;
        public string StudentFaculty { get; set; }
        public string CourseFaculty { get; set; }
        public string MajorSpeciality { get; set; }
        public decimal? Firstexam { get; set; }
        public decimal? Secondexam { get; set; }
        public decimal? Finalexam { get; set; }
        public decimal? Grade { get; set; }
        public decimal? Result { get; set; }
        public decimal? AverageGpa { get; set; }

    }
}

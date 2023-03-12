namespace WebApplication2.Models
{
    public class QueryModel
    {

        public string StudentNumber { get; set; } = null!;

        public string StudentName { get; set; } = null!;
        public short StudentFaculty { get; set; }

        public string CourseNumber { get; set; } = null!;

        public string CourseName { get; set; } = null!;

        public short? CourseFaculty { get; set; }

        public short? MajorSpecialty { get; set; }

        public decimal? Grade { get; set; }

        public short? Result { get; set; }

    }
}

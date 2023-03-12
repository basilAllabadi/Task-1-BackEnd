namespace WebApplication2.Models
{
    public class TotalGrade
    {
        public int  CourseId { get; set; }

        public string CourseNumber { get; set; } = null!;

        public string CourseName { get; set; } = null!;

        public long StudentId { get; set; }

        public string StudentNumber { get; set; } = null!;

        public string StudentName { get; set; } = null!;

        public decimal? Grade { get; set; }
        public decimal? creditHours { get; set; }


    }
}

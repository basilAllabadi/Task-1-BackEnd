
namespace WebApplication2.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string StudentNumber { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string? MobileNo { get; set; }

        public string? AcademicDegree { get; set; }

        public string? Faculty { get; set; }

        public string? MajorSpecialty { get; set; }

        public int? TotalCreditHours { get; set; }
        public decimal? AverageGpa { get; set; }

}
}

namespace WebApplication2.DbModels2;

public partial class GiCourse
{
    public int Id { get; set; }

    public string CourseNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Faculty { get; set; }

    public int CourseCreditHours { get; set; }

    public string? CourseDescription { get; set; }

    public int? CourseCategory { get; set; }

    public string? TeacherName { get; set; }

    public DateTime? CourseStartDate { get; set; }

    public DateTime? CourseEndDate { get; set; }


}

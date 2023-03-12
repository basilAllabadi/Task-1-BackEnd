namespace WebApplication2.DbModels2;

public partial class GiCoursesGrades1
{
    public int Id { get; set; }

    public int Courseid { get; set; }

    public int Studentid { get; set; }

    public int? Firstexam { get; set; }

    public int? Secondexam { get; set; }

    public int? Finalexam { get; set; }

    public decimal? Grade { get; set; }

    public decimal? Result { get; set; }


}

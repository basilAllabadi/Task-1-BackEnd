namespace WebApplication2.DbModels2;

public partial class GiStudent
{
    public int Id { get; set; }

    public string StudentNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public string? MobileNo { get; set; }

    public int? AcademicDegree { get; set; }

    public int? Faculty { get; set; }

    public int? MajorSpecialty { get; set; }

    public int? TotalCreditHours { get; set; }

    public decimal? AverageGpa { get; set; }

    public byte[]? HashedPassword { get; set; }

    public byte[]? PasswordSalt { get; set; }


}

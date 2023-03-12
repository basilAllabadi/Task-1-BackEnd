namespace WebApplication2.DbModels2;

public partial class Admin
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public byte[]? HashedPassword { get; set; }

    public byte[]? PasswordSalt { get; set; }
}

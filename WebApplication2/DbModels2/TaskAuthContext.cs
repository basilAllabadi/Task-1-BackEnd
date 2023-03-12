
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.DbModels2;

public partial class TaskAuthContext : DbContext
{
    public TaskAuthContext()
    {
    }

    public TaskAuthContext(DbContextOptions<TaskAuthContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Code> Codes { get; set; }

    public virtual DbSet<GiCourse> GiCourses { get; set; }

    public virtual DbSet<GiCoursesGrades1> GiCoursesGrades1s { get; set; }

    public virtual DbSet<GiStudent> GiStudents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BASIL-AL-LABADI\\SQLEXPRESS;Database=TaskAuth;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__admins__3214EC0769B6F4C9");

            entity.ToTable("admins");

            entity.HasIndex(e => e.UserName, "UQ__admins__C9F28456739A6DCA").IsUnique();

            entity.Property(e => e.HashedPassword).HasMaxLength(1024);
            entity.Property(e => e.PasswordSalt).HasMaxLength(1024);
            entity.Property(e => e.UserName)
                .HasMaxLength(55)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Code>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Codes__3214EC073C1F3708");

            entity.Property(e => e.CodeName)
                .HasMaxLength(55)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GiCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GiCourse__3214EC074A0F4AE9");

            entity.HasIndex(e => e.CourseNumber, "UQ__GiCourse__A98290EDD0A7C3B4").IsUnique();

            entity.Property(e => e.CourseDescription)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.CourseEndDate).HasColumnType("datetime");
            entity.Property(e => e.CourseNumber)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.CourseStartDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.TeacherName)
                .HasMaxLength(55)
                .IsUnicode(false);

 
        });

        modelBuilder.Entity<GiCoursesGrades1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GiCourse__3214EC0770CBA7EB");

            entity.ToTable("GiCoursesGrades1");

            entity.Property(e => e.Grade).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Result).HasColumnType("decimal(18, 0)");


        });

        modelBuilder.Entity<GiStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GiStuden__3214EC07DD12DA94");

            entity.HasIndex(e => e.StudentNumber, "UQ__GiStuden__DD81BF6CE718C593").IsUnique();

            entity.Property(e => e.AverageGpa).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.HashedPassword).HasMaxLength(1024);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt).HasMaxLength(1024);
            entity.Property(e => e.StudentNumber)
                .HasMaxLength(55)
                .IsUnicode(false);

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

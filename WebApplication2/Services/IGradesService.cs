using WebApplication2.DbModels2;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public interface IGradesService
    {
        Task<Response>? AddGrade(GiCoursesGrades1 request);
        Task<Response>? GetGrades();
        Task<Response>? UpdateGrade(int Id, GiCoursesGrades1 request);
        Task<Response>? DeleteGrade(int Id);
        Task<Response> GetStudentGrade(int Id);
        Task<Response> GetStudentGradeByStudent(int Id);


    }
}

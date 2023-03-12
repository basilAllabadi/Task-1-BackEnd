using WebApplication2.DbModels2;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public interface IStudentsService
    {
        Task<Response>? AddStudent(GiStudent request);
        Task<Response>? GetStudents();
        Task<Response>? UpdateStudent(int Id, GiStudent request);
        Task<Response>? GetStudent(int num);
        Task<Response>? DeleteStudent(int Id);
        Task<LoginResponse> StudentLogin(LoginClass request);
        Task<Response>? UpdateStudentByHim(int Id, StudentUpdateRequest request);
        Task<Response>? GetStudentByHim(int Id);
    }

}

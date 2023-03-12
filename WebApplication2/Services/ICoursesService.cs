using WebApplication2.DbModels2;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public interface ICoursesService
    {

        Task<Response>? AddCourse(GiCourse request);
        Task<Response>? GetCourses();
        Task<Response>? UpdateCourse(int Id, GiCourse request);
        Task<Response>? GetCourse(int num);
        Task<Response>? DeleteCourse(int Id);
    }
}

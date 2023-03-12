using WebApplication2.DbModels2;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public interface IAdminsService
    {
        Task<Admin>? AdminRegister(RegisterClass request);
        Task<LoginResponse> AdminLogin(LoginClass request);
    }
}

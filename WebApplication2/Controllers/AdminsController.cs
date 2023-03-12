using Microsoft.AspNetCore.Mvc;
using WebApplication2.DbModels2;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminsService _AdminssService;
        public AdminsController(IAdminsService adminsService)
        {

            _AdminssService = adminsService;
        }

        [HttpPost("register")]

        public async Task<ActionResult<Admin>>? AdminRegister(RegisterClass request)
            {
            var result = await _AdminssService.AdminRegister(request);
            return result;
            }

        [HttpPost("login")]

        public async Task<ActionResult<LoginResponse>> AdminLogin(LoginClass request)
        {
             var result = await _AdminssService.AdminLogin(request);

            if (result.Token == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

    }
}

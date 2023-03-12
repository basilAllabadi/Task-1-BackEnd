using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.DbModels2;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentsController : ControllerBase
    {
        private readonly IStudentsService _studentsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StudentsController(IStudentsService studentsService, IHttpContextAccessor httpContextAccessor)
        {

            _studentsService = studentsService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? AddStudent(GiStudent request)
        {
            var result = await _studentsService.AddStudent(request);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);

        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>>? StudentLogin(LoginClass request)
        {
            var result = await _studentsService.StudentLogin(request);
            if(result.Token==null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? GetStudents()
        {
            var result = await _studentsService.GetStudents();
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);
        }

        [HttpGet("search/{num}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? GetStudent(int num)
        {
            var result = await _studentsService.GetStudent(num);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result); 
        }

        [HttpGet("student/info")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<Response>>? GetStudentByHim()
        {

            if (_httpContextAccessor.HttpContext != null)
            {
                var Id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _studentsService.GetStudentByHim(int.Parse(Id));
                if (result.Result == "Error")
                {
                    return NotFound(result.Message);

                }
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("admin/{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? UpdateStudent(int Id, GiStudent request)
        {
            var result = await _studentsService.UpdateStudent(Id,request);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);

        }

        [HttpPut("student")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<Response>>? UpdateStudentByHim ( StudentUpdateRequest request)

        {

            if (_httpContextAccessor.HttpContext != null)
            {
                var Id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _studentsService.UpdateStudentByHim(int.Parse(Id),request);
                if (result.Result == "Error")
                {
                    return NotFound(result.Message);

                }
                return Ok(result);
            }
            return NotFound();

        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? DeleteStudent(int Id)
        {
            var result = await _studentsService.DeleteStudent(Id);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);

        }
    }
}

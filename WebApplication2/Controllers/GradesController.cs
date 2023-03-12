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
    public class GradesController : ControllerBase
    {
        private readonly IGradesService _gradesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GradesController(IGradesService gradesService, IHttpContextAccessor httpContextAccessor)
        {

            _gradesService = gradesService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? AddGrade(GiCoursesGrades1 request)
        {
            var result = await _gradesService.AddGrade(request);
            if(result.Result== "Error")
            {
               return NotFound(result.Message);

            }
            return Ok(result);

        }
  
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? GetGrades()
        {
            var result = await _gradesService.GetGrades();
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);
       

        }

        [HttpGet("admin/{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? GetStudentGrade(int Id)
        {
      
            var result = await _gradesService.GetStudentGrade(Id);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);

        }

        [HttpGet("student")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<Response>>? GetStudentGradeByStudent()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var Id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
         
                var result = await _gradesService.GetStudentGradeByStudent(int.Parse(Id));

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

        [HttpPut("{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? UpdateGrade(int Id, GiCoursesGrades1 request)
        {
            var result = await _gradesService.UpdateGrade(Id, request);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);

        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? DeleteGrade(int Id)
        {
            var result = await _gradesService.DeleteGrade(Id);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);

        }
    }
}

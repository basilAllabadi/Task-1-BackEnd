using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DbModels2;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class CoursesController : ControllerBase
    {

        private readonly ICoursesService _CoursesService;
        public CoursesController(ICoursesService coursesService)
        {

            _CoursesService = coursesService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? AddCourse(GiCourse request)
        {
            var result = await _CoursesService.AddCourse(request);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? GetCourses()
        {
            var result = await _CoursesService.GetCourses();
            if (result.Result == "Error")
            {
                return NotFound(result.Message);

            }
            return Ok(result);

        }

        [HttpGet("search/{num}")]
        public async Task<ActionResult<Response>>? GetCourse(int num)
        {
            var result = await _CoursesService.GetCourse(num);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? UpdateCourse(int Id, GiCourse request)
        {
            var result = await _CoursesService.UpdateCourse(Id, request);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Response>>? DeleteCourse(int Id)
        {
            var result = await _CoursesService.DeleteCourse(Id);
            if (result.Result == "Error")
            {
                return NotFound(result.Message);
            }
            return Ok(result); ;
        }
    }
}

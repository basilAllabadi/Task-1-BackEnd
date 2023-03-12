using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApplication2.DbModels2;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LOVController : ControllerBase
    {
        private readonly TaskAuthContext _context;
        public LOVController(TaskAuthContext context)
        {

            _context = context;
        }
        [HttpGet("/degrees")]
        public async Task<ActionResult<List<Code>>>? GetDegrees()
        {
            return await _context.Codes.Where(code=>code.MajorCode == 1).ToListAsync();
            

        }
        [HttpGet("/all")]
        public async Task<ActionResult<List<Code>>>? GetAll()
        {
            return await _context.Codes.ToListAsync();


        }
        [HttpGet("/faculties")]
        public async Task<ActionResult<List<Code>>>? GetFaculties()
        {
            return await _context.Codes.Where(code => code.MajorCode == 2).ToListAsync();


        }
        [HttpGet("/majors")]
        public async Task<ActionResult<List<Code>>>? GetMajors()
        {
            return await _context.Codes.Where(code => code.MajorCode == 3).ToListAsync();


        }
        [HttpGet("/genders")]
        public async Task<ActionResult<List<Code>>>? GetGenders()
        {
            return await _context.Codes.Where(code => code.MajorCode == 4).ToListAsync();


        }
    }
}

using Microsoft.EntityFrameworkCore;
using WebApplication2.DbModels2;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly TaskAuthContext _context;
        public CoursesService(TaskAuthContext context)
        {

            _context = context;
        }
        public async Task<Response>? AddCourse(GiCourse request)
        {

            var course2 = await _context.GiCourses.Where(course => course.CourseNumber == request.CourseNumber).FirstOrDefaultAsync();

            if(course2 !=null)
            {
                return new Response
                {
                    Message = $"Course With Course Number {request.CourseNumber} Is Already Exist",
                    Result = "Error"
                };
            }

            _context.GiCourses.AddAsync(request);

            await _context.SaveChangesAsync();
            return new Response
            {
                Message = $"Course Added Successfully",
                Result = await _context.GiCourses.ToListAsync()
            };
     
        }

        public async Task<Response>? GetCourse(int num)
        {
             var course = await (from c in _context.GiCourses
                                  join code1 in _context.Codes
                                   on c.Faculty equals code1.Id
                                  join code2 in _context.Codes
                                  on c.CourseCategory equals code2.Id
                                  where c.CourseNumber == num.ToString()
                                  select  new Course
                        {
                            Id = c.Id,
                            CourseNumber = c.CourseNumber,
                            Name = c.Name,
                            Faculty = code1.CodeName,
                            CourseCreditHours = c.CourseCreditHours,
                            CourseDescription = c.CourseDescription,
                            CourseCategory = code2.CodeName,
                            TeacherName = c.TeacherName,
                            CourseStartDate = c.CourseStartDate,
                            CourseEndDate = c.CourseEndDate
                        }).FirstOrDefaultAsync();
            if (course == null)
            {
                return new Response
                {
                    Message = $"There Is No Such Course",
                    Result = "Error"
                };
            }


            return new Response
            {
                Message = $"Course Get Successfully",
                Result = course
            };

        }
        public async Task<Response>? GetCourses()
        {
     

        
        var courses = await ( from c in _context.GiCourses
                       join code1 in _context.Codes
                       on c.Faculty equals code1.Id
                       join code2 in _context.Codes
                       on c.CourseCategory equals code2.Id
                       orderby c.CourseNumber
                       select new Course
                       {
                           Id = c.Id,
                           CourseNumber = c.CourseNumber,
                           Name = c.Name,
                           Faculty = code1.CodeName,
                           CourseCreditHours = c.CourseCreditHours,
                           CourseDescription = c.CourseDescription,
                           CourseCategory = code2.CodeName,
                           TeacherName = c.TeacherName,
                           CourseStartDate = c.CourseStartDate,
                           CourseEndDate = c.CourseEndDate
                       }).ToListAsync();

            if (courses.Count == 0)
            {
                return new Response
                {
                    Message = $"There Is No Added Courses Yet !",
                    Result = "Error"
                };
            }


            return new Response
            {
                Message = $"Courses Get Successfully",
                Result = courses
            };
           
        }

        public async Task<Response>? UpdateCourse(int Id, GiCourse request)
        {
            var course = await (from s in _context.GiCourses
                                 where s.Id == Id
                                 select s).FirstOrDefaultAsync();
            if (course == null)
            {
                return new Response
                {
                    Message = $"Course With Id {Id} Not Found",
                    Result = "Error"
                };
              
       
            }
      
            
            course.CourseNumber = request.CourseNumber;
            course.Name = request.Name;
            course.Faculty = request.Faculty;
            course.CourseCreditHours = request.CourseCreditHours;
            course.CourseDescription = request.CourseDescription;
            course.CourseCategory = request.CourseCategory;
            course.TeacherName = request.TeacherName;
            course.CourseStartDate = request.CourseStartDate;
            course.CourseEndDate = request.CourseEndDate;
       
            await _context.SaveChangesAsync();

            return new Response
            {
                Message = $"Course Updated Successfully",
                Result = await _context.GiCourses.ToListAsync()
        };
 
        }

        public async Task<Response>? DeleteCourse(int Id)
        {
            var course = await _context.GiCourses.FindAsync(Id);
            var grade = await _context.GiCoursesGrades1s.Where(grade1=>grade1.Courseid == Id).FirstOrDefaultAsync();


            if (course == null)
            {
                return new Response
                {
                    Message = $"Course With Id {Id} Not Found",
                    Result = "Error"
                };
            }

            if (grade != null)
            {
                return new Response
                {
                    Message = $"Please Delete The Course Grades To be able to Delete The Course",
                    Result = "Error"
                };
            }
            _context.GiCourses.Remove(course);
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = $"Course Deleted Successfully",
                Result = await _context.GiCourses.ToListAsync()
            };

        }
    }
}

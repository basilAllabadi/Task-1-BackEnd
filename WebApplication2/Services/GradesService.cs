
using Microsoft.EntityFrameworkCore;
using WebApplication2.DbModels2;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class GradesService : IGradesService
    {
        private readonly TaskAuthContext _context;
        public GradesService(TaskAuthContext context)
        {

            _context = context;
        }
        public async Task<Response>? AddGrade(GiCoursesGrades1 request)
        {
            var student = await _context.GiStudents.FindAsync(request.Studentid);
            if(student == null)
            {
                return new Response
                {
                    Message = $"No Such Student",
                    Result = "Error"
                };
            }
            var course = await _context.GiCourses.FindAsync(request.Courseid);
            if (course == null)
            {
                return new Response
                {
                    Message = $"No Such Course",
                    Result = "Error"
                };
            }

            var newCourse = await _context.GiCoursesGrades1s.Where(grade => grade.Courseid == request.Courseid && grade.Studentid == request.Studentid).FirstOrDefaultAsync();
           

            if (newCourse != null)
            {
                return new Response
                {
                    Message = $"{$"The Grade For Student Id {request.Studentid}  In Course Id {request.Courseid} Is Already Entered"}",
                    Result = "Error"
                };

            }
            else
            {
                var grade = new GiCoursesGrades1
                {

                    Courseid = request.Courseid,
                    Studentid = request.Studentid,
                    Firstexam = request.Firstexam,
                    Secondexam = request.Secondexam,
                    Finalexam = request.Finalexam,
                    Grade = (request.Firstexam != null) && (request.Secondexam != null) && (request.Finalexam != null) ? request.Firstexam + request.Secondexam + request.Finalexam : null,
                    Result = (request.Finalexam + request.Secondexam + request.Finalexam) >= 50 ? 1 : 2
                };
                await _context.GiCoursesGrades1s.AddAsync(grade);

               
                await _context.SaveChangesAsync();

                if (grade.Grade != null)
                {
                    var gpa = await (from grades in _context.GiCoursesGrades1s
                                     join c in _context.GiCourses
                                     on grades.Courseid equals c.Id
                                     where grades.Studentid == request.Studentid
                                     select grades.Grade * c.CourseCreditHours).SumAsync();
                    var hours = await (from grades in _context.GiCoursesGrades1s
                                     join c in _context.GiCourses
                                     on grades.Courseid equals c.Id
                                     where grades.Studentid == request.Studentid
                                     where grades.Grade != null
                                     select c.CourseCreditHours).SumAsync();
                    var EnteredStudent = await _context.GiStudents.FindAsync(request.Studentid);
                    EnteredStudent.AverageGpa = Math.Round((decimal)(gpa / hours));
                    EnteredStudent.TotalCreditHours= hours;

                }


                await _context.SaveChangesAsync();

                return new Response
                {
                    Message = $"Grade Added Successfully",
                    Result = await _context.GiCoursesGrades1s.ToListAsync()
            }; 
            }
        }
        public async Task<Response>? GetGrades()
        {

            var grades = await (from g in _context.GiCoursesGrades1s
                                join student in _context.GiStudents
                                       on g.Studentid equals student.Id
                                join course in _context.GiCourses
                                       on g.Courseid equals course.Id
                                       join code1 in _context.Codes
                                       on student.Faculty equals code1.Id
                                        join code2 in _context.Codes
                                       on student.MajorSpecialty equals code2.Id
                                        join code3 in _context.Codes
                                       on course.Faculty equals code3.Id

                                select new Grades
                                {
                                    Id = g.Id,
                                    StudentName = student.Name,
                                    StudentNumber = student.StudentNumber,
                                    StudentFaculty = code1.CodeName,
                                    CourseName = course.Name,
                                    CourseNumber = course.CourseNumber,
                                    CourseFaculty = code3.CodeName,
                                    MajorSpeciality = code2.CodeName,
                                    Firstexam = g.Firstexam,
                                    Secondexam = g.Secondexam,
                                    Finalexam = g.Finalexam,
                                    Grade = g.Finalexam + g.Secondexam + g.Firstexam,
                                    Result = g.Result,
                                    AverageGpa =student.AverageGpa,
                                }).ToListAsync();
            if(grades.Count ==0)
            {
                return new Response
                {
                    Message = $"No Added Grades Yet",
                    Result = "Error"
                };
            }
            return  new Response
            {
                Message = $"All Grades",
                Result = grades
            };

        }
        
        public async Task<Response>? UpdateGrade(int Id, GiCoursesGrades1 request)
        {

            var grade = await _context.GiCoursesGrades1s.FindAsync(Id);

            if (grade == null)
            {
                return new Response
                {
                    Message = $"There Is No Such Grade With Id {Id} to Update",
                    Result = "Error"
                };
            }

            grade.Firstexam = request.Firstexam;
            grade.Secondexam = request.Secondexam;
            grade.Finalexam = request.Finalexam;
            grade.Grade = (request.Firstexam != null) && (request.Secondexam != null) && (request.Finalexam != null) ? request.Firstexam + request.Secondexam + request.Finalexam : null;
            grade.Result = (request.Finalexam + request.Secondexam + request.Finalexam) >= 50 ? 1 : 2;


            await _context.SaveChangesAsync();

            if (grade.Grade != null)
            {
                var gpa = await (from grades in _context.GiCoursesGrades1s
                                 join course in _context.GiCourses
                                 on grades.Courseid equals course.Id
                                 where grades.Studentid == grade.Studentid
                                 select grades.Grade * course.CourseCreditHours).SumAsync();
                var hours = await (from grades in _context.GiCoursesGrades1s
                                   join course in _context.GiCourses
                                   on grades.Courseid equals course.Id
                                   where grades.Grade != null
                                   where grades.Studentid == grade.Studentid
                                   select course.CourseCreditHours).SumAsync();
                var student = await _context.GiStudents.FindAsync(grade.Studentid);
                student.AverageGpa = Math.Round((decimal)(gpa / hours));
                student.TotalCreditHours = hours;

            }
          
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = $"The Grade Updated Successfully",
                Result = grade
            };
        }
        public async Task<Response>? DeleteGrade(int Id)
        {

            var grade = await _context.GiCoursesGrades1s.FindAsync(Id);

            if (grade == null)
{
                return new Response
                {
                    Message = $"There Is No Such Grade With Id {Id} to Update",
                    Result = "Error"
                };
            }

            if (grade.Grade != null)
            {
                _context.GiCoursesGrades1s.Remove(grade);
                await _context.SaveChangesAsync();
               

                var gpa = await (from grades in _context.GiCoursesGrades1s
                                 join course in _context.GiCourses
                                 on grades.Courseid equals course.Id
                                 where grades.Studentid == grade.Studentid
                                 select grades.Grade * course.CourseCreditHours).SumAsync();
                var hours = await (from grades in _context.GiCoursesGrades1s
                                   join course in _context.GiCourses
                                   on grades.Courseid equals course.Id
                                   where grades.Studentid == grade.Studentid
                                   where grades.Grade != null
                                   select course.CourseCreditHours).SumAsync();
                var student = await _context.GiStudents.FindAsync(grade.Studentid);

                if (hours == 0)
                {
                    student.AverageGpa = null;
                    student.TotalCreditHours = null;
                }
                else
                {
                    student.AverageGpa = Math.Round((decimal)(gpa / hours));
                    student.TotalCreditHours = hours;
                }

            }
            else
            {
                _context.GiCoursesGrades1s.Remove(grade);
            }



            await _context.SaveChangesAsync();
            return new Response
            {
                Message = $"Grade Deleted Successfully",
                Result = await _context.GiCoursesGrades1s.ToListAsync()
            };
          
        }
        public async Task<Response> GetStudentGradeByStudent(int Id)
        {
            var grades = await (from grade1 in _context.GiCoursesGrades1s
                                join student in _context.GiStudents
                                on grade1.Studentid equals student.Id
                                join course in _context.GiCourses
                               on grade1.Courseid equals course.Id
                                join code1 in _context.Codes
                                      on student.Faculty equals code1.Id
                                join code2 in _context.Codes
                               on student.MajorSpecialty equals code2.Id
                                join code3 in _context.Codes
                               on course.Faculty equals code3.Id
                                where grade1.Studentid == Id
                                select new Grades
                                {
                                    Id = grade1.Id,
                                    StudentName = student.Name,
                                    StudentNumber = student.StudentNumber,
                                    StudentFaculty = code1.CodeName,
                                    CourseName = course.Name,
                                    CourseNumber = course.CourseNumber,
                                    CourseFaculty = code3.CodeName,
                                    MajorSpeciality = code2.CodeName,
                                    Firstexam = grade1.Firstexam,
                                    Secondexam = grade1.Secondexam,
                                    Finalexam = grade1.Finalexam,
                                    Grade = grade1.Grade,
                                    Result = grade1.Result,
                                    AverageGpa = student.AverageGpa,
                                }


                         ).ToListAsync();

            if (grades.Count == 0)
            {
                return new Response
                {
                    Message = $"There Is No Such Grade For Student Id {Id} to Get",
                    Result = "Error"
                };
            }
            return new Response
            {
                Message = $"Student Grades",
                Result = grades
            };
        }
        public async Task<Response> GetStudentGrade(int Id)
        {
        var grades = await (from grade1 in _context.GiCoursesGrades1s
                            join student in _context.GiStudents
                            on grade1.Studentid equals student.Id
                            join course in _context.GiCourses
                           on grade1.Courseid equals course.Id
                            where grade1.Studentid == Id
                            where grade1.Grade !=null
                            select new TotalGrade
                           {
            CourseId= course.Id,
            CourseName = course.Name,
            CourseNumber = course.CourseNumber,
            StudentId = student.Id,
            StudentNumber=  student.StudentNumber,
            StudentName = student.Name,
            Grade=grade1.Grade,
            creditHours=course.CourseCreditHours,
    }
                          ).ToListAsync();

            if(grades.Count==0)
            {
                return new Response
                {
                    Message = $"There Is No Such Grade For Student Id {Id} to Get",
                    Result = "Error"
                };
            }
            return new Response
            {
                Message = $"Student Grades",
                Result = grades
            };
        }
    }
}


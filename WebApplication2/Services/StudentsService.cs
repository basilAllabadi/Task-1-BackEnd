using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApplication2.DbModels2;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly TaskAuthContext _context;
        private readonly IConfiguration _configuration;
        public StudentsService(TaskAuthContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<Response>? AddStudent(GiStudent request)
        {

            var student = await _context.GiStudents.Where(s => s.StudentNumber == request.StudentNumber).FirstOrDefaultAsync();

            if (student != null)
            {
                return new Response
                {
                    Message = $"Student With Student Number {request.StudentNumber} Is Already Exist",
                    Result = "Error"
                };
            }
            CreatePasswordHash("111111", out byte[] passwordHash, out byte[] passwordSalt);

            var newStudent = new GiStudent
            {
                StudentNumber = request.StudentNumber,
                Name = request.Name,
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                MobileNo = request.MobileNo,
                AcademicDegree = request.AcademicDegree,
                Faculty = request.Faculty,
                MajorSpecialty = request.MajorSpecialty,
                TotalCreditHours = request.TotalCreditHours,
                AverageGpa = request.AverageGpa,
                PasswordSalt = passwordSalt,
                HashedPassword = passwordHash,
            }
           ;

            _context.GiStudents.AddAsync(newStudent);


            await _context.SaveChangesAsync();
            return new Response
            {
                Message = $"Student Added Successfully",
                Result = await _context.GiStudents.ToListAsync()
            };
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(GiStudent student)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, student.Name),
                new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
                new Claim(ClaimTypes.Role,"student")
    

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;


        }
        public async Task<LoginResponse> StudentLogin(LoginClass request)
        {

            var student = await _context.GiStudents.Where(s => s.Name == request.UserName).FirstOrDefaultAsync();
            if (student == null)
            {
                return new LoginResponse
                {
                    Status = 400,
                    Message = "Worng UserName",
                    Token=null
                };
            }
            else
            {
                if (!VerifyPasswordHash(request.Password, student.HashedPassword, student.PasswordSalt))
                {
                    return new LoginResponse
                    {
                        Status = 400,
                        Message = "Worng Password",
                        Token = null
                    };
                }
                var token = CreateToken(student);

                return new LoginResponse
                {
                    Status = 400,
                    Message = "Valid Credenitals",
                    Token = token
                };

            }
        }
        public async Task<Response>? GetStudent(int num)
        {

            var student = await (from s in _context.GiStudents
                                  join code1 in _context.Codes
                                    on s.Gender equals code1.Id
                                  join code2 in _context.Codes
                                  on s.MajorSpecialty equals code2.Id
                                  join code3 in _context.Codes
                                  on s.AcademicDegree equals code3.Id
                                  join code4 in _context.Codes
                                  on s.Faculty equals code4.Id
                                  where s.StudentNumber == num.ToString()
                                  select new Student
                                  {
                                      Id = s.Id,
                                      StudentNumber = s.StudentNumber,
                                      Name = s.Name,
                                      Gender = code1.CodeName,
                                      BirthDate = s.BirthDate,
                                      MobileNo = s.MobileNo,
                                      AcademicDegree = code3.CodeName,
                                      Faculty = code4.CodeName,
                                      MajorSpecialty = code2.CodeName,
                                      TotalCreditHours = s.TotalCreditHours,
                                      AverageGpa = s.AverageGpa
                                  }).FirstOrDefaultAsync();

            if (student == null)
            {
                return new Response
                {
                    Message = $"There Is No Such Student",
                    Result = "Error"
                };
            }

            return new Response
            {
                Message = $"Student Get Successfully",
                Result = student
            };

        }
        public async Task<Response>? GetStudentByHim(int Id)
        {

            var student = await (from s in _context.GiStudents
                                  join code1 in _context.Codes
                                  on s.Gender equals code1.Id
                                  join code2 in _context.Codes
                                  on s.MajorSpecialty equals code2.Id
                                  join code3 in _context.Codes
                                  on s.AcademicDegree equals code3.Id
                                  join code4 in _context.Codes
                                  on s.Faculty equals code4.Id
                                  where s.Id == Id
                                  select new Student
                                  {
                                      Id = s.Id,
                                      StudentNumber = s.StudentNumber,
                                      Name = s.Name,
                                      Gender = code1.CodeName,
                                      BirthDate = s.BirthDate,
                                      MobileNo = s.MobileNo,
                                      AcademicDegree = code3.CodeName,
                                      Faculty = code4.CodeName,
                                      MajorSpecialty = code2.CodeName,
                                      TotalCreditHours = s.TotalCreditHours,
                                      AverageGpa = s.AverageGpa
                                  }).FirstOrDefaultAsync();

            if (student == null)
            {
                return new Response
                {
                    Message = $"There Is No Such Student",
                    Result = "Error"
                };
            }

            return new Response
            {
                Message = $"Student Get Successfully",
                Result = student
            };

        }
        public async Task<Response>? GetStudents()
        {
        

            var students = await (from s in _context.GiStudents
                                  join code1 in _context.Codes
                                  on s.Gender equals code1.Id
                                  join code2 in _context.Codes
                                  on s.MajorSpecialty equals code2.Id
                                  join code3 in _context.Codes
                                  on s.AcademicDegree equals code3.Id
                                  join code4 in _context.Codes
                                  on s.Faculty equals code4.Id
                                  orderby s.StudentNumber
                                  select new Student
                                  { Id = s.Id,
                                      StudentNumber = s.StudentNumber,
                                      Name = s.Name,
                                      Gender = code1.CodeName,
                                      BirthDate = s.BirthDate,
                                      MobileNo = s.MobileNo,
                                      AcademicDegree = code3.CodeName,
                                      Faculty = code4.CodeName,
                                      MajorSpecialty = code2.CodeName,
                                      TotalCreditHours = s.TotalCreditHours,
                                    AverageGpa = s.AverageGpa
                                  }).ToListAsync();
      

            if (students.Count == 0)
            {
                return new Response
                {
                    Message = $"There Is No Added Students Yet !!",
                    Result = "Error"
                };
            }

            return new Response
            {
                Message = $"Students Get Successfully",
                Result = students
            };
        }

        public async Task<Response>? UpdateStudent(int Id, GiStudent request)
        {

           
            var student = await (from s in _context.GiStudents
                                 where s.Id == Id
                                 select s).FirstOrDefaultAsync();
            if (student == null)
            {
                return new Response
                {
                    Message = $"There Is No Such Student !!",
                    Result = "Error"
                };
            }

            student.StudentNumber = request.StudentNumber;
            student.Name = request.Name;
            student.Gender = request.Gender;
            student.BirthDate = request.BirthDate;
            student.MobileNo = request.MobileNo;
            student.AcademicDegree = request.AcademicDegree;
            student.Faculty = request.Faculty;
            student.MajorSpecialty = request.MajorSpecialty;
            student.TotalCreditHours = request.TotalCreditHours;

            await _context.SaveChangesAsync();
            return new Response
            {
                Message = $"Student Updated Successfully",
                Result = await _context.GiStudents.ToListAsync()
        };
       
        }
        public async Task<Response>? UpdateStudentByHim(int Id, StudentUpdateRequest request)
        {


            var student = await (from s in _context.GiStudents
                                 where s.Id == Id
                                 select s).FirstOrDefaultAsync();
            if (student == null)
            {
                return new Response
                {
                    Message = $"There Is No Such Student !!",
                    Result = "Error"
                };
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            student.HashedPassword = passwordHash;
            student.PasswordSalt= passwordSalt;


            await _context.SaveChangesAsync();
            return new Response
            {
                Message = $"Student Updated Successfully",
                Result = await _context.GiStudents.ToListAsync()
            };

        }
        public async Task<Response>? DeleteStudent(int Id)
        {
            var student = await _context.GiStudents.FindAsync(Id);
            var grade = await _context.GiCoursesGrades1s.Where(grade1 => grade1.Studentid == Id).FirstOrDefaultAsync();

            if (student == null)
            {
                return new Response
                {
                    Message = $"There Is No Such Student !!",
                    Result = "Error"
                };
            }

            if (grade != null)
            {
                return new Response
                {
                    Message = $"Please Delete The Student Grades To be able to Delete The Student",
                    Result = "Error"
                };
            }
            _context.GiStudents.Remove(student);
            await _context.SaveChangesAsync();
            return new Response
            {
                Message = $"Student Deleted Successfully",
                Result = await _context.GiStudents.ToListAsync()
            };
        }



    }

}

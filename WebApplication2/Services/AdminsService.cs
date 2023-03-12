using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApplication2.DbModels2;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class AdminsService :IAdminsService
    {
        private readonly TaskAuthContext _context;
        private readonly IConfiguration _configuration;
        public AdminsService(TaskAuthContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<Admin>? AdminRegister(RegisterClass request)
        {
            var user = await _context.Admins.Where(admin => admin.UserName == request.UserName).FirstOrDefaultAsync();
            if (user != null)
            {
                return null;
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var newUser = new Admin
            {
   
                UserName = request.UserName,
                HashedPassword = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Admins.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;

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
        private string CreateToken(Admin admin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.UserName),
                   new Claim(ClaimTypes.Role,"admin")

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
       public async Task<LoginResponse> AdminLogin(LoginClass request)
        {

            var admin = await _context.Admins.Where(admin1 => admin1.UserName == request.UserName).FirstOrDefaultAsync();
            if (admin == null)
            {
                return new LoginResponse
                {
                    Status = 400,
                    Message = "Worng Username",
                    Token = null,

                };
            }

            else
            {
                if (!VerifyPasswordHash(request.Password, admin.HashedPassword, admin.PasswordSalt))
                {
                    return new LoginResponse
                    {
                        Status = 400,
                        Message = "Worng Password",
                        Token = null,

                    };
                }
                var token = CreateToken(admin);


                return new LoginResponse
                {
                    Status = 200,
                    Message = "Valid Credentials",
                    Token = token,
                };

            }
        }
    }
}

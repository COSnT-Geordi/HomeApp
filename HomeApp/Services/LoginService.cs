using HomeApp.SqlModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.Data;
using HomeApp.Common;
using HomeApp.Models;

namespace HomeApp.Services
{
    public class LoginService
    {
        private IConfiguration _configuration;
        private HomeDbContext _context;
        public LoginService(HomeDbContext context,IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signUpRequest"></param>
        /// <returns>string exists,success,failed</returns>
        public string SignUp(SignUpRequest signUpRequest) 
        {
            var user = _context?.Users?.ToList()?.SingleOrDefault(u => u.Username == signUpRequest.UserName);
            if (user != null) return "exists";
            try
            {
                _context.Users.Add(new User 
                {
                    ID = 0,
                    Username = signUpRequest.UserName,
                    
                    Creation_date = DateTime.Now.ToUniversalTime(),
                    Password = Hash(signUpRequest.Password.Trim()),
                });
                _context.SaveChanges();
                return "success";
            }
            catch (Exception)
            {
                return "failed";
            }

        }

        public string Login(LoginRequest request)
        {
            var passwordHashed = Hash(request.Password.Trim());
           var user =  _context?.Users?.ToList()?.SingleOrDefault(u => u.Username == request.Email && u?.Password?.Trim() == passwordHashed);

            if(user == null) return string.Empty;

            string? pass = _configuration["Auth:HashPass"];
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.Username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(pass!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public string Hash(string password)
        {
            string? pass = _configuration["Auth:HashPass"];
            if (string.IsNullOrEmpty(pass)) throw new InvalidOperationException("The Secret key is empty! Please first set a secret key!");
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(pass));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(hash);
        }


    }
}

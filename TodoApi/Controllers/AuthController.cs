using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApi.Models;
namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ToDoDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ToDoDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 1. הרשמה - Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            // בדיקה אם המשתמש כבר קיים
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return BadRequest("משתמש זה כבר קיים במערכת");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "ההרשמה בוצעה בהצלחה" });
        }

        // 2. התחברות - Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(User loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("שם משתמש או סיסמה שגויים");
            }

            // יצירת ה-Token
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "default_secret_key_at_least_32_chars"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
using api_lib.Data;
using api_lib.Models;
using api_lib.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api_lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("list-users")]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public IActionResult ListUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var userIdValue))
            {
                return BadRequest(new { error = "ID do usuário inválido." });
            }

            var currentUser = _context.Users.FirstOrDefault(u => u.Id == userIdValue);

            if (currentUser == null || !currentUser.IsAdmin)
            {
                return Forbid();
            }

            var users = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.IsAdmin,
                    u.CreatedAt
                })
                .ToList();

            return Ok(users);
        }

        [HttpPost("register")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var userIdValue))
            {
                return BadRequest(new { error = "ID do usuário inválido." });
            }

            var currentUser = _context.Users.FirstOrDefault(u => u.Id == userIdValue);

            if (currentUser == null || !currentUser.IsAdmin)
            {
                return Forbid();
            }

            string name = request.Name;
            string email = request.Email;
            string password = request.Password;
            string confirmPassword = request.ConfirmPassword;
            bool isAdmin = request.IsAdmin;
            DateTime dateOfBirth = request.DateOfBirth;

            if (_context.Users.Any(u => u.Email == email))
            {
                return BadRequest(new { error = "Email já está em uso!" });
            }

            if (password != confirmPassword)
            {
                return BadRequest(new { error = "As senhas não correspondem!" });
            }

            if (dateOfBirth > DateTime.UtcNow)
            {
                return BadRequest(new { error = "A data de nascimento não pode ser no futuro!" });
            }

            var user = new User
            {
                Name = name,
                Email = email,
                Password = HashPassword(password),
                IsAdmin = isAdmin,
                DateOfBirth = dateOfBirth,
                CreatedAt = DateTime.UtcNow
            };

            _context.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            string email = request.Email;
            string password = request.Password;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || !verifyPassword(password, user.Password)) {
                return Unauthorized(new { error = "Credenciais inválidas." });
            }

            var token = GenerateJwtToken(user);

            return Ok(
                new { 
                    message = "Login realizado com sucesso!", 
                    token
                }
             );
        }

        private bool verifyPassword(string password, string hashedPassword) {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);

            var hashedInput = Convert.ToBase64String(hash);

            return hashedPassword == hashedInput;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("IsAdmin", user.IsAdmin.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

     
    }
}

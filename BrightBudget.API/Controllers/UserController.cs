using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BrightBudget.API.Models;
using BrightBudget.Core.Models;
using BrightBudget.Infrastructure.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BrightBudget.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public UserController(IUserRepository repo, IConfiguration config, ILogger<UserController> logger)
            : base(logger)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelState();
            }
            model.Email = model.Email.Trim();
            var existingUser = await _repo.GetByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return Error("This email address is already registered.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var newUser = new User
            {
                Email = model.Email,
                PasswordHash = passwordHash,
            };

            await _repo.AddAsync(newUser);
            await _repo.SaveAsync();
            return Success("Account created successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Trim and sanitize
            var email = request.Email.Trim().ToLowerInvariant();
            var password = request.Password.Trim();

            if (!ModelState.IsValid)
                return InvalidModelState();

            var user = await _repo.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return Error("Invalid email or password.");

            // Create JWT claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Success(new { token = tokenString }, "Login successful.");
        }
    }
}
using BrightBudget.Core.Models;
using BrightBudget.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BrightBudget.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo, ILogger<UserController> logger)
            : base(logger)
        {
            _repo = repo;
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


    }

}

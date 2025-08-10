using BrightBudget.API.Dtos.Auth;
using BrightBudget.API.Extensions;
using BrightBudget.API.Filters;
using BrightBudget.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BrightBudget.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return Fail("Email already exists");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            return Fail(result.Errors.First().Description);

            return Success(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized(new { message = "Invalid credentials" });

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            
            // Store refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                int.Parse(_config["Jwt:RefreshTokenExpiryDays"] ?? "7")
            );
            
            await _userManager.UpdateAsync(user);

            return Success(new { 
                accessToken, 
                refreshToken,
                expiresIn = int.Parse(_config["Jwt:AccessTokenExpiryMinutes"] ?? "15") * 60
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            try
            {
                // Use extension method to get user from expired token
                var user = await HttpContext.GetCurrentUserFromExpiredToken(dto.AccessToken, _userManager, _config);
                if (user == null)
                    return Unauthorized(new { message = "Invalid access token" });

                // Validate refresh token
                if (user.RefreshToken != dto.RefreshToken || 
                    user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    return Unauthorized(new { message = "Invalid or expired refresh token" });

                // Generate new tokens
                var newAccessToken = GenerateAccessToken(user);
                var newRefreshToken = GenerateRefreshToken();
                
                // Update refresh token in database
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                    int.Parse(_config["Jwt:RefreshTokenExpiryDays"] ?? "7")
                );
                
                await _userManager.UpdateAsync(user);

                return Success(new { 
                    accessToken = newAccessToken, 
                    refreshToken = newRefreshToken,
                    expiresIn = int.Parse(_config["Jwt:AccessTokenExpiryMinutes"] ?? "15") * 60
                });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        [ServiceFilter(typeof(CurrentUserFilter))]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter
                // Invalidate refresh token
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);

                return Success(new { message = "Logged out successfully" });
            }
            catch (Exception)
            {
                return Success(new { message = "Logged out successfully" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        [ServiceFilter(typeof(CurrentUserFilter))]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter

                // Verify current password
                if (!await _userManager.CheckPasswordAsync(user, dto.CurrentPassword))
                    return BadRequest(new { message = "Current password is incorrect" });

                // Validate new password
                var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();
                if (passwordValidator != null)
                {
                    var validationResult = await passwordValidator.ValidateAsync(_userManager, user, dto.NewPassword);
                    if (!validationResult.Succeeded)
                    {
                        var errors = validationResult.Errors.Select(e => e.Description);
                        return BadRequest(new { message = "New password validation failed", errors });
                    }
                }

                // Change password
                var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { message = "Failed to change password", errors });
                }

                // Invalidate all refresh tokens for security
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);

                return Success(new { message = "Password changed successfully. Please log in again." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while changing password", error = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize]
        [ServiceFilter(typeof(CurrentUserFilter))]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var user = HttpContext.GetCurrentUser()!; // User is guaranteed to exist due to filter

                return Success(new { 
                    id = user.Id,
                    email = user.Email,
                    userName = user.UserName
                });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
        }

        private string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:AccessTokenExpiryMinutes"] ?? "15")
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

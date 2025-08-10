using BrightBudget.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BrightBudget.API.Middleware
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public CurrentUserMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var token = ExtractTokenFromHeader(context);
                if (!string.IsNullOrEmpty(token))
                {
                    var user = await GetUserFromToken(token, userManager);
                    if (user != null)
                    {
                        context.Items["CurrentUser"] = user;
                    }
                }
            }
            catch (Exception)
            {
                // Silently continue if token processing fails
                // The controller will handle authentication as needed
            }

            await _next(context);
        }

        private string? ExtractTokenFromHeader(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return null;

            return authHeader.Substring("Bearer ".Length);
        }

        private async Task<ApplicationUser?> GetUserFromToken(string token, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
                    ),
                    ValidateLifetime = false // We want to get the user even if the token is expired
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                
                if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return null;

                return await userManager.FindByIdAsync(userId);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    // Extension method for easy registration
    public static class CurrentUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CurrentUserMiddleware>();
        }
    }
}

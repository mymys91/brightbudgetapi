using BrightBudget.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BrightBudget.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static ApplicationUser? GetCurrentUser(this HttpContext context)
        {
            return context.Items["CurrentUser"] as ApplicationUser;
        }

        public static string? GetCurrentUserId(this HttpContext context)
        {
            return context.GetCurrentUser()?.Id;
        }

        public static async Task<ApplicationUser?> GetCurrentUserFromExpiredToken(
            this HttpContext context, 
            string expiredToken, 
            UserManager<ApplicationUser> userManager,
            IConfiguration config)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
                    ),
                    ValidateLifetime = false // We want to get the principal even if the token is expired
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out SecurityToken securityToken);
                
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
}

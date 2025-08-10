using Microsoft.AspNetCore.Identity;

namespace BrightBudget.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Wallet>? Wallets { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
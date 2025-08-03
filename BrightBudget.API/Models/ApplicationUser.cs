using Microsoft.AspNetCore.Identity;

namespace BrightBudget.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Wallet>? Wallets { get; set; }
    }
}
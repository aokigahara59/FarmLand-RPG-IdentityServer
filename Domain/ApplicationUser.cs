using Microsoft.AspNetCore.Identity;

namespace Application.Common.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; } = "";
        public List<UserRefreshToken> RefreshTokens { get; set; }
    }
}

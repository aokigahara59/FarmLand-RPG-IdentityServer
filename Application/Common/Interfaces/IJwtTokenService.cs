using Application.Common.Models;
using Application.Common.Models.DTO;
using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        JwtToken GenerateToken(ApplicationUser user);
        ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
    }
}

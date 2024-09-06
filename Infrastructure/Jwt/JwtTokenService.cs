using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.DTO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtSettins)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtSettins.Value;
        }

        public JwtToken GenerateToken(ApplicationUser user)
        {
            var signningCredentials = new SigningCredentials(
                GetKey(),
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("nickname", user.Nickname)
            };

            var securiryToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: signningCredentials
                );

            return new JwtToken(new JwtSecurityTokenHandler()
                .WriteToken(securiryToken),
                _jwtSettings.ExpiryMinutes);
        }

        public ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParametrs = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetKey(),
                ValidateLifetime = false,
            };

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, tokenValidationParametrs, out var securiryToken);

                if (securiryToken is not JwtSecurityToken jwtSecurityToken || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }

           
        }

        private SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        }
    }
}

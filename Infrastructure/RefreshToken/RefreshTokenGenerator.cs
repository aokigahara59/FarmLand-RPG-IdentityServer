using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Infrastructure.RefreshToken
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly RefreshTokenSettings _refreshTokenSettings;

        public RefreshTokenGenerator(IOptions<RefreshTokenSettings> refreshTokenSettings)
        {
            _refreshTokenSettings = refreshTokenSettings.Value;
        }

        public Application.Common.Models.DTO.RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            var token = Convert.ToBase64String(randomNumber);

            return new Application.Common.Models.DTO.RefreshToken(token, _refreshTokenSettings.ExpiryDays);
        }
    }
}

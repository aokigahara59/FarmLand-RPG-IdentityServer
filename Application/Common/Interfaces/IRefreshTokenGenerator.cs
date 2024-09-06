using Application.Common.Models.DTO;

namespace Application.Common.Interfaces
{
    public interface IRefreshTokenGenerator
    {
        RefreshToken GenerateRefreshToken();
    }
}

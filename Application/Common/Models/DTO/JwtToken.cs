namespace Application.Common.Models.DTO
{
    public record JwtToken(string Token, long ExpiryMinutes);
}

namespace Application.Common.Models.DTO
{
    public record AuthenticationResponse(JwtToken JwtToken, RefreshToken RefreshToken);
}

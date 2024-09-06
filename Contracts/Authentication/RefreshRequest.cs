namespace Contracts.Authentication
{
    public record RefreshRequest(string ExpiredJwtToken, string RefreshToken);
}

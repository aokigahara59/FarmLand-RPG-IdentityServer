namespace Contracts.Authentication
{
    public record RegisterRequest(string Nickname, string Email, string Password);
}

using Application.Common.Models.DTO;
using ErrorOr;
using MediatR;

namespace Application.Authentication.Commands.Refresh
{
    public record RefreshCommand(
        string ExpiredJwtToken, 
        string RefreshToken) : IRequest<ErrorOr<AuthenticationResponse>>;
}

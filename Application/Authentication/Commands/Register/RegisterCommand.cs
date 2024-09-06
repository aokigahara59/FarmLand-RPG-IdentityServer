using Application.Common.Models.DTO;
using ErrorOr;
using MediatR;

namespace Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string Nickname,
        string Email,
        string Password) : IRequest<ErrorOr<AuthenticationResponse>>;

}

using Application.Common.Models.DTO;
using ErrorOr;
using MediatR;

namespace Application.User.Commands.ChangeUsername
{
    public record ChangeUsernameCommand(
        string Email,
        string NewUsername) : IRequest<ErrorOr<JwtToken>>;
}

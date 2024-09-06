using Application.Common.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Application.Common.Errors;
using Application.Common.Models.DTO;
using Application.Common.Models;

namespace Application.User.Commands.ChangeUsername
{
    public class ChangeUsernameCommandHandler
        : IRequestHandler<ChangeUsernameCommand, ErrorOr<JwtToken>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenGenerator;

        public ChangeUsernameCommandHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<JwtToken>> Handle(ChangeUsernameCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            user.Nickname = request.NewUsername;
            await _userManager.UpdateAsync(user);

            JwtToken token = _jwtTokenGenerator.GenerateToken(user);

            return token;
        }
    }
}

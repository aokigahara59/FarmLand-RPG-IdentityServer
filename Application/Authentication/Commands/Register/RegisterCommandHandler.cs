using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.DTO;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler 
        : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IDateTimeProvider _dateProvider;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IDateTimeProvider dateProvider,
            IMapper mapper)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _dateProvider = dateProvider;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                Nickname = request.Nickname,
                UserName = request.Email
            };

            JwtToken token = _jwtTokenGenerator.GenerateToken(user);
            RefreshToken refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            var userRefreshToken = _mapper.Map<UserRefreshToken>(refreshToken);
            userRefreshToken.ApplicationUserId = user.Id;

            user.RefreshTokens = [];
            user.RefreshTokens.Add(userRefreshToken);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new AuthenticationResponse(token, refreshToken);
            }

            return result.Errors.Select(x => Error.Failure(
                code: x.Code, description: x.Description)).ToList().ToErrorOr<AuthenticationResponse>();
        }
    }
}

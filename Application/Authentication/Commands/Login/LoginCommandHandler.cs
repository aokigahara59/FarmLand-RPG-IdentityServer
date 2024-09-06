using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Models.DTO;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Application.Common.Models;

namespace Application.Authentication.Commands.Login
{
    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, ErrorOr<AuthenticationResponse>>
    {
        private readonly IJwtTokenService _jwtTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IDateTimeProvider _dateProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;


        public LoginCommandHandler(
            IJwtTokenService jwtTokenGenerator,
            UserManager<ApplicationUser> userManager,
            IRefreshTokenGenerator refreshTokenGenerator,
            IDateTimeProvider dateProvider,
            IMapper mapper)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _refreshTokenGenerator = refreshTokenGenerator;
            _dateProvider = dateProvider;
            _mapper = mapper;
        }


        public async Task<ErrorOr<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {       
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user is null)
            {
                return Errors.Authentication.InvalidCredentials;
            }
            
            var passwordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
            
            if (!passwordCorrect)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            JwtToken token = _jwtTokenGenerator.GenerateToken(user);
            RefreshToken refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            var userRefreshToken = _mapper.Map<UserRefreshToken>(refreshToken);
            userRefreshToken.ApplicationUserId = user.Id;

            user.RefreshTokens.Add(userRefreshToken);
 
            await _userManager.UpdateAsync(user);

            return new AuthenticationResponse(token, refreshToken);
        }
    }
}

using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.DTO;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Authentication.Commands.Refresh
{
    public class RefreshCommandHandler
        : IRequestHandler<RefreshCommand, ErrorOr<AuthenticationResponse>>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IMapper _mapper;

        public RefreshCommandHandler(IJwtTokenService jwtTokenService,
            UserManager<ApplicationUser> userManager,
            IDateTimeProvider dateTimeProvider,
            IRefreshTokenGenerator refreshTokenGenerator,
            IMapper mapper)
        {
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
            _dateTimeProvider = dateTimeProvider;
            _refreshTokenGenerator = refreshTokenGenerator;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AuthenticationResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            string token = request.ExpiredJwtToken;
            string oldRefreshToken = request.RefreshToken;
            ClaimsPrincipal? principal =_jwtTokenService.GetClaimsPrincipalFromExpiredToken(token);

            if (principal is null)
            {
                return Errors.Authentication.InvalidCredentials;
            } 

            string email = principal.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            var user = await _userManager.Users.
                Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user is null)
            {
                return Errors.Authentication.InvalidToken;
            }

            UserRefreshToken? savedRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == oldRefreshToken);

            if (savedRefreshToken is null ||
                savedRefreshToken.Token != oldRefreshToken
                || savedRefreshToken.ExpiryTime <= _dateTimeProvider.UtcNow)
            {
                return Errors.Authentication.InvalidToken;
            }

            JwtToken newToken = _jwtTokenService.GenerateToken(user);  
            RefreshToken newRefreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            var userRefreshToken = _mapper.Map<UserRefreshToken>(newRefreshToken);
            userRefreshToken.ApplicationUserId = user.Id;

            user.RefreshTokens.Remove(savedRefreshToken);
            user.RefreshTokens.Add(userRefreshToken);

            await _userManager.UpdateAsync(user);

            return new AuthenticationResponse(newToken, newRefreshToken);
        }
    }
}

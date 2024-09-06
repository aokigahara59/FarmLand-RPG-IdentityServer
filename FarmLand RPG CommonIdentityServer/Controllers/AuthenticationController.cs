using Application.Authentication.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Contracts.Authentication;
using MapsterMapper;
using Application.Authentication.Commands.Login;
using ErrorOr;
using Application.Authentication.Commands.Refresh;
using Application.Common.Models.DTO;

namespace FarmLand_RPG_CommonIdentityServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public AuthenticationController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);
            ErrorOr<AuthenticationResponse> result = await _sender.Send(command);

            return result.Match(Ok, Problem);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = _mapper.Map<LoginCommand>(request);
            ErrorOr<AuthenticationResponse> result = await _sender.Send(command);

            return result.Match(Ok, Problem);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest request)
        {
            var command = _mapper.Map<RefreshCommand>(request);
            ErrorOr<AuthenticationResponse> result = await _sender.Send(command);

            return result.Match(Ok, Problem);
        }
    }
}

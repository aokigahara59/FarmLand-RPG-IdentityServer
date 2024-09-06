using Application.Common.Models.DTO;
using Application.User.Commands.ChangeUsername;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmLand_RPG_CommonIdentityServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ApiController
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }
      
        [HttpPost("update-username")]
        public async Task<IActionResult> UpdateName(string name)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var command = new ChangeUsernameCommand(email, name);

            ErrorOr<JwtToken> result = await _sender.Send(command);

            return result.Match(Ok, Problem);
        }
    }
}

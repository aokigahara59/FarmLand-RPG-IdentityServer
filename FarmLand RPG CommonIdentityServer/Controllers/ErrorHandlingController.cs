using Microsoft.AspNetCore.Mvc;

namespace FarmLand_RPG_CommonIdentityServer.Controllers
{
    [ApiController]
    public class ErrorHandlingController : ControllerBase
    {

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult HandleError()
        {
            return Problem();
        }
    }
}

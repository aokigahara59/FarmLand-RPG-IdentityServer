using ErrorOr;
using FarmLand_RPG_CommonIdentityServer.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FarmLand_RPG_CommonIdentityServer.Controllers
{
    
    [ApiController]
    public class ApiController : ControllerBase
    {
        [NonAction]
        public IActionResult Problem(List<Error> errors)
        {
            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;
            var firstError = errors[0];
            return Problem(firstError);
        }

        [NonAction]
        private IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Failure => StatusCodes.Status417ExpectationFailed,
                ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
                ErrorType.Unauthorized => StatusCodes.Status203NonAuthoritative,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };
            return Problem(statusCode: statusCode, title: error.Description);
        }

        [NonAction]
        private IActionResult ValidationProblem(List<Error> errors)
        {
            if (errors.Count() is 0) return Problem();

            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}

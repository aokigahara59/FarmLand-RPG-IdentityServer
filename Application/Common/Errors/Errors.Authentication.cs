using ErrorOr;

namespace Application.Common.Errors
{
    public static partial class Errors
    {
        public static class Authentication
        {
            public static Error InvalidCredentials = Error.Validation(
                code: "Auth.InvalidCredentials",
                description: "Invalid Credentials"
                );

            public static Error InvalidToken = Error.Validation(
               code: "Auth.InvalidToken",
               description: "Invalid access or refresh token"
               );
        }
    }
}

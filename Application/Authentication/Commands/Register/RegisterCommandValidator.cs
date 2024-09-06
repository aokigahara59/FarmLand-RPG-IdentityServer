using FluentValidation;

namespace Application.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Nickname)
                .NotEmpty()
                .Length(5, 16)
                .Matches(@"^[a-zA-Z0-9_]+$");

            RuleFor(x => x.Email).EmailAddress().NotEmpty();

            RuleFor(x => x.Password).NotEmpty().MinimumLength(7);
        }
    }
}

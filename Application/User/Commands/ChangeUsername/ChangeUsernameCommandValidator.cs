using FluentValidation;

namespace Application.User.Commands.ChangeUsername
{
    public class ChangeUsernameCommandValidator : AbstractValidator<ChangeUsernameCommand>
    {
        public ChangeUsernameCommandValidator()
        {
            RuleFor(x => x.NewUsername)
                .NotEmpty()
                .Length(5, 16)
                .Matches(@"^[a-zA-Z0-9_]+$");

            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}

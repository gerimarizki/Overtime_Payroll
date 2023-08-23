using FluentValidation;
using server.DTOs.Accounts;

namespace server.Utilities.Validators
{

    public class LoginValidator : AbstractValidator<LoginAccountDto>
    {
        public LoginValidator()
        {
            RuleFor(p => p.Email)
               .NotEmpty()
               .EmailAddress();

            RuleFor(p => p.Password)
               .NotEmpty();
        }
    }
}

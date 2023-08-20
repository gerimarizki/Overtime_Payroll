using FluentValidation;
using Overtime_Payroll.DTOs.Accounts;

namespace Overtime_Payroll.Utilities.Validators
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

using FluentValidation;
using server.Contracts;
using server.DTOs.Accounts;

namespace server.Utilities.Validators
{

    public class RegisterValidation : AbstractValidator<RegisterAccountDto>
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IAccountRepository _accountRepository;

        public RegisterValidation(IEmployeeRepository employeeRepository, IAccountRepository accountRepository)
        {
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;

            RuleFor(p => p.FirstName)
               .NotEmpty();

            RuleFor(p => p.BirthDate)
               .NotEmpty()
               .LessThanOrEqualTo(DateTime.Now.AddYears(-10));

            RuleFor(p => p.Gender)
               .NotNull()
               .IsInEnum();

            RuleFor(p => p.HiringDate)
               .NotEmpty();

            RuleFor(p => p.Email)
               .NotEmpty()
               .Must(BeUniqueAccount).WithMessage(p => $"{p.Email} already registered")
               .EmailAddress();

            RuleFor(p => p.PhoneNumber)
               .NotEmpty()
               .Must(BeUniqueProperty).WithMessage(p => $"{p.PhoneNumber} already registered")
               .Matches(@"^\+[1-9]\d{1,20}$").WithMessage("Invalid phone number format.");

            RuleFor(p => p.Password)
                .NotEmpty()
                .Matches(@"^(?=.*[0-9])(?=.*[A-Z]).{8,}$").WithMessage("Password invalid! Passwords must have at least 1 upper case and 1 number and 8 Digits");

            RuleFor(p => p.ConfirmPassword)
              .NotEmpty()
              .Equal(p => p.Password).WithMessage("Password and Confirm Password do not match.");
        }

        private bool BeUniqueProperty(string property)
        {
            return _employeeRepository.IsDuplicateValue(property);
        }

        private bool BeUniqueAccount(string property)
        {
            return _accountRepository.IsDuplicateValue(property);
        }
    }
}
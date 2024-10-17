using FluentValidation;

using LoanAgent.Application.Common.Constants;

namespace LoanAgent.Application.User.Commands.CreateAdminUser
{
    public class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
    {
        public CreateAdminCommandValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .MinimumLength(UserEntityValidationConstants.UsernameMinLength)
                .MaximumLength(UserEntityValidationConstants.UsernameMaxLength);

            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.Password)
                .NotEmpty()
                .NotEqual(p => p.Email)
                .NotEqual(p => p.Username)
                .MinimumLength(UserEntityValidationConstants.PasswordMinLength);

            RuleFor(u => u.FirstName)
                .NotEmpty()
                .MaximumLength(UserEntityValidationConstants.FirstNameMaxLength);

            RuleFor(u => u.LastName)
                .NotEmpty()
                .MaximumLength(UserEntityValidationConstants.LastNameMaxLength);

            RuleFor(u => u.IdNumber)
                .NotEmpty()
                .Length(UserEntityValidationConstants.IdNumberMinLength, UserEntityValidationConstants.IdNumberMaxLength);

            RuleFor(u => u.DateOfBirth)
                .LessThan(DateTime.Now)
                .WithMessage("Date of Birth must be in the past.");
        }
    }
}

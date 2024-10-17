using FluentValidation;

namespace LoanAgent.Application.Loan.Commands.CreateLoan;

public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(v => v.LoanAmount).GreaterThan(0).WithMessage("Loan amount must be greater than zero.");
        RuleFor(v => v.Currency).IsInEnum().WithMessage("Invalid currency.");
        RuleFor(v => v.StartDate).LessThan(v => v.EndDate).WithMessage("Start date must be before end date.");
    }
}

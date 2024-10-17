using FluentValidation;

namespace LoanAgent.Application.Loan.Commands.ChangeLoanStatus;

public class ChangeLoanStatusCommandValidator : AbstractValidator<ChangeLoanStatusCommand>
{
    public ChangeLoanStatusCommandValidator()
    {
        RuleFor(v => v.LoanId).NotEmpty();
        RuleFor(v => v.NewLoanState).IsInEnum().WithMessage("Invalid loan state.");
    }
}

using FluentValidation;

namespace LoanAgent.Application.Loan.Commands.DeleteLoan;

public class DeleteLoanCommandValidator : AbstractValidator<DeleteLoanCommand>
{
    public DeleteLoanCommandValidator()
    {
        RuleFor(v => v.LoanId).NotEmpty();
    }
}

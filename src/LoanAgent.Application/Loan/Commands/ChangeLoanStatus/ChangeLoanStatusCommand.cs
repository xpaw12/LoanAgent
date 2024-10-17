using LoanAgent.Domain.Enums;

using MediatR;

namespace LoanAgent.Application.Loan.Commands.ChangeLoanStatus;

public class ChangeLoanStatusCommand : IRequest<Unit>
{
    public Guid LoanId { get; set; }
    public LoanState NewLoanState { get; set; }

    public ChangeLoanStatusCommand(Guid loanId, LoanState newLoanState)
    {
        LoanId = loanId;
        NewLoanState = newLoanState;
    }
}

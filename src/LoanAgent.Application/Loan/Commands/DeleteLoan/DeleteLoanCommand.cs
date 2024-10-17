using MediatR;

namespace LoanAgent.Application.Loan.Commands.DeleteLoan;

public class DeleteLoanCommand : IRequest<Unit>
{
    public Guid LoanId { get; set; }
}

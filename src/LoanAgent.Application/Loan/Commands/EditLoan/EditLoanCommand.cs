using LoanAgent.Domain.Enums;

using MediatR;

namespace LoanAgent.Application.Loan.Commands.EditLoan;

public class EditLoanCommand : IRequest<Guid>
{
    public Guid LoanId { get; set; }
    public decimal LoanAmount { get; set; }
    public Currency Currency { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LoanType LoanType { get; set; }
}

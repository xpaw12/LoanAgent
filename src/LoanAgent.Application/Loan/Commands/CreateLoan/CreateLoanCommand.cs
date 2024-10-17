using LoanAgent.Domain.Enums;

using MediatR;

namespace LoanAgent.Application.Loan.Commands.CreateLoan;

public class CreateLoanCommand : IRequest<Guid>
{
    public decimal LoanAmount { get; set; }
    public Currency Currency { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LoanType LoanType { get; set; }
}

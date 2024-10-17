using LoanAgent.Application.Common.Dtos;

using MediatR;

namespace LoanAgent.Application.Loan.Queries.GetLoansForAdmin;

public class GetLoansForAdminQuery : IRequest<List<LoanDto>>
{
}

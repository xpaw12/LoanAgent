using LoanAgent.Application.Common.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace LoanAgent.Application.Loan.Queries.GetLoansByUserId;

public class GetLoansByUserIdQuery : IRequest<List<LoanDto>>
{
    [FromQuery(Name = "userId")]
    public Guid? UserId { get; set; }
}

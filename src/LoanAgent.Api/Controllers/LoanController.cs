using LoanAgent.Application.Loan.Commands.ChangeLoanStatus;
using LoanAgent.Application.Loan.Commands.CreateLoan;
using LoanAgent.Application.Loan.Commands.DeleteLoan;
using LoanAgent.Application.Loan.Commands.EditLoan;
using LoanAgent.Application.Loan.Queries.GetLoansByUserId;
using LoanAgent.Application.Loan.Queries.GetLoansForAdmin;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanAgent.Api.Controllers;

[Route("api/loans")]
public class LoanController : ApiControllerBase
{
    [HttpGet]
    public async Task<IResult> GetLoansByUserId([FromQuery] GetLoansByUserIdQuery query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "1")]
    public async Task<IResult> GetLoansForAdmin(GetLoansForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    [HttpPost("create")]
    public async Task<IResult> CreateLoan([FromBody] CreateLoanCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IResult> EditLoan([FromBody] EditLoanCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    [HttpPut("change-status")]
    public async Task<IResult> ChangeLoanStatus([FromBody] ChangeLoanStatusCommand command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IResult> DeleteLoan([FromBody] DeleteLoanCommand command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }
}

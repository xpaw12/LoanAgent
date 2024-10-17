using LoanAgent.Application.User.Commands.CreateAdminUser;
using LoanAgent.Application.User.Commands.CreateUser;
using LoanAgent.Application.User.Commands.Login;
using LoanAgent.Application.User.Queries.GetUserDetailsById;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanAgent.Api.Controllers;

[Route("api/users")]
public class UserController : ApiControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IResult> Login([FromBody]LoginUserCommand command, CancellationToken cancellationToken)
    {
        var accessToken = await Mediator.Send(command, cancellationToken);
        return Results.Ok(accessToken);
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IResult> CreateUser([FromBody]CreateUserCommand command, CancellationToken cancellationToken)
    {
        var accessToken = await Mediator.Send(command, cancellationToken);
        return Results.Ok(accessToken);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IResult> GetUserDetails(GetUserDetailsByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    [HttpPost("create-admin")]
    [Authorize(Roles = "1")]
    public async Task<IResult> CreateAdmin([FromBody]CreateAdminCommand command, CancellationToken cancellationToken)
    {
        var accessToken = await Mediator.Send(command, cancellationToken);
        return Results.Ok(accessToken);
    }
}

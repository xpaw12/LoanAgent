using LoanAgent.Application.Common.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace LoanAgent.Application.User.Queries.GetUserDetailsById;

public class GetUserDetailsByIdQuery : IRequest<UserDto>
{
    [FromRoute(Name = "userId")]
    public Guid UserId { get; set; }
}

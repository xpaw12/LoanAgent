using MediatR;

namespace LoanAgent.Application.User.Commands.Login;

public class LoginUserCommand : IRequest<string>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

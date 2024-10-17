using MediatR;

namespace LoanAgent.Application.User.Commands.CreateUser;

public class CreateUserCommand : IRequest<string>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string IdNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}
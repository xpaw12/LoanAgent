namespace LoanAgent.Application.User.Commands.CreateUser;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message)
        : base(message)
    {
    }
}
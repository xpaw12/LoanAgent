using LoanAgent.Application.Common.Interfaces.Repositories;
using LoanAgent.Application.Common.Security.Jwt.Enums;
using LoanAgent.Application.Common.Security.Jwt.Interfaces;
using LoanAgent.Application.Common.Security.Password;

using MediatR;

using System.Security.Authentication;

namespace LoanAgent.Application.User.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(u => u.Username == request.UserName || request.UserName == u.Email,
            cancellationToken);

        if (user is null || !PasswordManager.IsValidPassword(request.Password, user.Password, user.Salt))
        {
            throw new InvalidCredentialException("Invalid username or password");
        }

        return await _jwtTokenService.GenerateTokenAsync(user.Id, (UserRole)user.UserRole);
    }
}

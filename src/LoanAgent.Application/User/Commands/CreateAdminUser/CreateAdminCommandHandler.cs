using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Application.Common.Security.Jwt.Enums;
using LoanAgent.Application.Common.Security.Jwt.Interfaces;
using LoanAgent.Application.Common.Security.Password;
using LoanAgent.Application.User.Commands.CreateUser;
using LoanAgent.Domain.Entities;

using MediatR;

namespace LoanAgent.Application.User.Commands.CreateAdminUser;

public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public CreateAdminCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _unitOfWork.UserRepository.DoesUserExist(request.Username, request.Email);

        if (userExists)
        {
            throw new UserAlreadyExistsException("Admin with the specified username or email already exists.");
        }

        var (passwordHash, passwordSalt) = PasswordManager.CreatePasswordHash(request.Password);

        var admin = new UserEntity
        {
            Username = request.Username,
            Email = request.Email,
            Password = passwordHash,
            Salt = passwordSalt,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IdNumber = request.IdNumber,
            DateOfBirth = request.DateOfBirth,
            UserRole = (Domain.Enums.UserRole)UserRole.Admin,
            IsActive = true,
            Deleted = false
        };

        await _unitOfWork.UserRepository.AddAsync(admin, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _jwtTokenService.GenerateTokenAsync(admin.Id, UserRole.Admin);
    }
}

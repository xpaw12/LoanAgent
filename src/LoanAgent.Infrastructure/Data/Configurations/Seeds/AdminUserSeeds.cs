using LoanAgent.Application.Common.Security.Password;
using LoanAgent.Domain.Entities;
using LoanAgent.Domain.Enums;

namespace LoanAgent.Infrastructure.Data.Configurations.Seeds;

public static class AdminUserSeeds
{
    public static readonly UserEntity[] Data;

    static AdminUserSeeds()
    {
        var (passwordHash, passwordSalt) = PasswordManager.CreatePasswordHash("Password1!");

        Data = new[]
        {
            new UserEntity
            {
                Id = Guid.Parse("D860EFCA-22D9-47FD-8249-791BA61B07C7"),
                Username = "admin",
                Email = "admin@loanagent.com",
                Password = passwordHash,
                Salt = passwordSalt,
                FirstName = "Admin",
                LastName = "User",
                IdNumber = "00000000",
                DateOfBirth = new DateTime(1980, 1, 1),
                IsActive = true,
                Deleted = false,
                UserRole = UserRole.Admin
            }
        };
    }
}

using LoanAgent.Application.Common.Security.Jwt.Enums;

namespace LoanAgent.Application.Common.Security.Jwt.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(
        Guid userId,
        UserRole role);
}

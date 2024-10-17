namespace LoanAgent.Application.Common.Interfaces.Helpers;

public interface ICurrentUser
{
    Guid Id { get; }
    bool IsInRole(string commaSeparatedRoles);
}
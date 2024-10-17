using LoanAgent.Application.Common.Dtos;
using LoanAgent.Domain.Entities;

namespace LoanAgent.Application.User.Queries.GetUserDetailsById;

public static class UserDetailsMapper
{
    public static UserDto ToUserDtoMapper(this UserEntity user)
    {
        return new UserDto
        {
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IdNumber = user.IdNumber,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            Deleted = user.Deleted,
            UserRole = user.UserRole,
            Loans = user.Loans is null 
                ? new List<LoanDto>()
                : user.Loans?.Select(loan => loan.ToLoanDtoMapper()).ToList()
        };
    }

    public static LoanDto ToLoanDtoMapper(this LoanEntity loan)
    {
        return new LoanDto
        {
            LoanId = loan.Id,
            LoanAmount = loan.LoanAmount,
            Currency = loan.Currency.ToString(),
            StartDate = loan.StartDate,
            EndDate = loan.EndDate,
            LoanType = loan.LoanType.ToString(),
            LoanState = loan.LoanState.ToString(),
            CreatedDateTime = loan.CreatedDateTime,
            CreatedById = loan.CreatedById,
            UpdatedDateTime = loan.UpdatedDateTime,
            UpdatedById = loan.UpdatedById,
            DeletedDateTime = loan.DeletedDateTime,
            DeletedById = loan.DeletedById
        };
    }
}

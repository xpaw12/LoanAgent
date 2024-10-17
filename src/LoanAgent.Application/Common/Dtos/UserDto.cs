using LoanAgent.Domain.Enums;

namespace LoanAgent.Application.Common.Dtos;

public class UserDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string IdNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public UserRole UserRole { get; set; }
    public ICollection<LoanDto>? Loans { get; set; } = new List<LoanDto>();
    public bool IsActive { get; set; }
    public bool Deleted { get; set; }
}

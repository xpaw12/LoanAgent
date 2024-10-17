using LoanAgent.Domain.Common;
using LoanAgent.Domain.Enums;

namespace LoanAgent.Domain.Entities;

public class UserEntity : EntityBase
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string IdNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public UserRole UserRole { get; set; }
    public virtual ICollection<LoanEntity> Loans { get; set; } = new List<LoanEntity>();
}

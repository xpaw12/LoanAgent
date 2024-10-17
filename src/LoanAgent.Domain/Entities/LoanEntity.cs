using LoanAgent.Domain.Common;
using LoanAgent.Domain.Enums;

namespace LoanAgent.Domain.Entities;

public class LoanEntity : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal LoanAmount { get; set; }
    public Currency Currency { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LoanType LoanType { get; set; }
    public LoanState LoanState { get; set; }
    public virtual UserEntity User { get; set; } = null!;
}

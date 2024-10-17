namespace LoanAgent.Domain.Common;

public abstract class EntityBase
{
    public bool IsActive { get; set; } = true;
    public bool Deleted { get; set; }
}

namespace LoanAgent.Domain.Common;

public abstract class AuditableEntity : EntityBase
{
    public DateTime CreatedDateTime { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public Guid? UpdatedById { get; set; }
    public DateTime? DeletedDateTime { get; set; }
    public Guid? DeletedById { get; set; }
}

namespace LoanAgent.Application.Common.Dtos;

public class LoanDto
{
    public Guid LoanId { get; set; }
    public decimal LoanAmount { get; set; }
    public string Currency { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string LoanType { get; set; } = null!;
    public string LoanState { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public Guid? UpdatedById { get; set; }
    public DateTime? DeletedDateTime { get; set; }
    public Guid? DeletedById { get; set; }
}

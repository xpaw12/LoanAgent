using LoanAgent.Domain.Common;
using LoanAgent.Domain.Enums;

using System.Text.Json.Serialization;

namespace LoanAgent.Domain.Entities;

public class LoanEntity : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal LoanAmount { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Currency Currency { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LoanType LoanType { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LoanState LoanState { get; set; }
    public virtual UserEntity User { get; set; } = null!;
}

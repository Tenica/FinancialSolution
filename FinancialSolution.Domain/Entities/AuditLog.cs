using FinancialSolution.Domain.Common;

namespace FinancialSolution.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid CustomerId { get; set; }

    public string Action { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string? IpAddress { get; set; }

    public DateTime PerformedAt { get; set; }

    // Navigation Property
    public Customer Customer { get; set; } = default!;
}
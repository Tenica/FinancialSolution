using FinancialSolution.Domain.Common;

namespace FinancialSolution.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid CustomerId { get; set; }

    public string Token { get; set; } = default!;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public Customer Customer { get; set; } = default!;
}
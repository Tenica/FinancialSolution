using FinancialSolution.Domain.Common;

namespace FinancialSolution.Domain.Entities;

public class PasswordResetToken : BaseEntity
{
    public Guid CustomerId { get; set; }

    public string Token { get; set; } = default!;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public Customer Customer { get; set; } = default!;
}
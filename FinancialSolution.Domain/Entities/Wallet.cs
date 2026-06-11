using FinancialSolution.Domain.Common;

namespace FinancialSolution.Domain.Entities;

public class Wallet : BaseEntity
{
    public Guid CustomerId { get; set; }

    public Guid CurrencyId { get; set; }

    public string AccountNumber { get; set; } = default!;

    public decimal Balance { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public Customer Customer { get; set; } = default!;

    public Currency Currency { get; set; } = default!;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
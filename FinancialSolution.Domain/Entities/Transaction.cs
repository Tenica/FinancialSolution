using FinancialSolution.Domain.Common;
using FinancialSolution.Domain.Enums;

namespace FinancialSolution.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid WalletId { get; set; }

    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public TransactionStatus Status { get; set; }

    public string Reference { get; set; } = default!;

    // Navigation Property
    public Wallet Wallet { get; set; } = default!;
}
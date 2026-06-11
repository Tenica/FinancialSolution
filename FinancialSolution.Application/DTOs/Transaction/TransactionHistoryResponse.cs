namespace FinancialSolution.Application.DTOs.Transaction;

public class TransactionHistoryResponse
{
    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = default!;

    public string Status { get; set; } = default!;

    public string Reference { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}
namespace FinancialSolution.Application.DTOs.Transaction;

public class TransactionResponse
{
    public string Reference { get; set; } = default!;

    public decimal Amount { get; set; }

    public string Status { get; set; } = default!;
}
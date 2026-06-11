namespace FinancialSolution.Application.DTOs.Transaction;

public class StatementResponse
{
    public string AccountNumber { get; set; } = default!;

    public string Currency { get; set; } = default!;

    public decimal CurrentBalance { get; set; }

    public List<TransactionHistoryResponse>
        Transactions
    { get; set; } = new();
}
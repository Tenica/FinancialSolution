namespace FinancialSolution.Application.DTOs.Transaction;

public class TransferRequest
{
    public string SenderAccountNumber { get; set; } = default!;

    public string ReceiverAccountNumber { get; set; } = default!;

    public decimal Amount { get; set; }
}
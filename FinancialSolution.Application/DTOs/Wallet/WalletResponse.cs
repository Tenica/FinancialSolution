namespace FinancialSolution.Application.DTOs.Wallet;

public class WalletResponse
{
    public string AccountNumber { get; set; } = default!;

    public decimal Balance { get; set; }

    public string Currency { get; set; } = default!;
}
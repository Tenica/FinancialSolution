namespace FinancialSolution.Application.DTOs.Wallet;

public class FundWalletRequest
{
    public string AccountNumber { get; set; } = default!;

    public decimal Amount { get; set; }
}
using FinancialSolution.Application.DTOs.Wallet;

namespace FinancialSolution.Application.Interfaces.Services;

public interface IWalletService
{
    Task<WalletResponse?> GetMyWalletAsync(Guid customerId);

    Task FundWalletAsync(FundWalletRequest request);
}
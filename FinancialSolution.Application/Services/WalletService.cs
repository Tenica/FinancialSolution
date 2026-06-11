using FinancialSolution.Application.DTOs.Wallet;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;

namespace FinancialSolution.Application.Services;

public class WalletService : IWalletService
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IWalletRepository _walletRepository;

    private readonly IUnitOfWork _unitOfWork;

    public WalletService(ICustomerRepository customerRepository, IWalletRepository walletRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _walletRepository = walletRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WalletResponse?> GetMyWalletAsync(Guid customerId)
    {
        var customer =
            await _customerRepository
                .GetCustomerWithWalletAsync(customerId);

        if (customer == null)
        {
            return null;
        }

        var wallet = customer.Wallets.FirstOrDefault();

        if (wallet == null)
        {
            return null;
        }

        return new WalletResponse
        {
            AccountNumber = wallet.AccountNumber,
            Balance = wallet.Balance,
            Currency = wallet.Currency.Code
        };
    }

    public async Task FundWalletAsync(
    FundWalletRequest request)
    {
        var wallet =
            await _walletRepository
                .GetByAccountNumberAsync(request.AccountNumber);

        if (wallet == null)
        {
            throw new Exception("Wallet not found.");
        }

        if (request.Amount <= 0)
        {
            throw new Exception("Amount must be greater than zero.");
        }

        wallet.Balance += request.Amount;

        await _walletRepository.UpdateAsync(wallet);

        await _unitOfWork.SaveChangesAsync();
    }
}
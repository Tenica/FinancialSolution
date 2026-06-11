using FinancialSolution.Domain.Entities;

namespace FinancialSolution.Application.Interfaces.Repositories;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetByAccountNumberAsync(string accountNumber);

    Task CreateWalletAsync(
    Guid customerId,
    Guid currencyId,
    string accountNumber);

    Task UpdateAsync(Wallet wallet);

    Task<Wallet?> GetByCustomerIdAsync(Guid customerId);
}


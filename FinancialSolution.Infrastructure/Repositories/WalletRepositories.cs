using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialSolution.Infrastructure.Repositories;

public class WalletRepository
    : GenericRepository<Wallet>, IWalletRepository
{
    public WalletRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Wallet?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _context.Wallets
            .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
    }

    public async Task CreateWalletAsync(
    Guid customerId,
    Guid currencyId,
    string accountNumber)
    {
        await _context.Database.ExecuteSqlRawAsync(

            "EXEC sp_CreateWallet @p0, @p1, @p2",
            customerId,
            currencyId,
            accountNumber);
    }

    public Task UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);

        return Task.CompletedTask;
    }

    public async Task<Wallet?> GetByCustomerIdAsync(
    Guid customerId)
    {
        return await _context.Wallets
             .Include(x => x.Currency)
            .FirstOrDefaultAsync(
                x => x.CustomerId == customerId);
    }
}



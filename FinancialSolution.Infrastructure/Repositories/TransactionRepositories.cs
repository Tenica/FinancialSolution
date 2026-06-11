using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialSolution.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteTransferAsync(
        string senderAccountNumber,
        string receiverAccountNumber,
        decimal amount,
        string reference)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_TransferFunds @p0, @p1, @p2, @p3",

            senderAccountNumber,

            receiverAccountNumber,

            amount,

            reference);
    }

    public async Task<List<Transaction>>
       GetWalletTransactionsAsync(
           Guid walletId,
           int page,
           int pageSize)
    {
        return await _context.Transactions
            .Where(x => x.WalletId == walletId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int>
    GetTransactionCountAsync(Guid walletId)
    {
        return await _context.Transactions
            .CountAsync(x => x.WalletId == walletId);
    }
}

//How it accesses it: _context is your ApplicationDbContext(the secure bridge to SQL Server). .Transactions is a DbSet, which is Entity Framework's representation of your physical Transactions table in the database.

//What it does: At this exact moment, it does nothing to the database. It doesn't load any data. It simply points at the table and says, "Get ready, we are about to build a query for this table."
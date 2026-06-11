using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialSolution.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction =
            await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }
}


//This is the contract for your Unit of Work(the "Restaurant Waiter's Notepad"). It promises that whoever implements this interface can do two things:

//SaveChangesAsync() : This tells the database to execute all the standard inserts/updates you staged in memory.It returns an int representing exactly how many rows in the SQL database were changed.

//BeginTransactionAsync(): This is the "Vault Door." It tells the database to suspend normal auto-saving and start a highly monitored, lockable transaction.It returns an IDbContextTransaction object, which is the physical "key" you use to either Commit() or Rollback() later.
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace FinancialSolution.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;

    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context; // "Save the bridge connection so we can save changes later."
        _dbSet = context.Set<T>(); // "Find the specific table lane for whatever T happens to be, so we can query it."
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }


}

//base(context): This is how you pass the baton.The.NET Dependency Injection engine hands the ApplicationDbContext to your ScheduledTransferRepository.By adding : base(context), you immediately hand that database connection up the chain to the GenericRepository so it can do its setup
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialSolution.Infrastructure.Repositories;

public class RefreshTokenRepository
    : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        RefreshToken refreshToken)
    {
        await _context.RefreshTokens
            .AddAsync(refreshToken);
    }

    public async Task<RefreshToken?>
        GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(
                x => x.Token == token);
    }

    public async Task<List<RefreshToken>>
    GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.RefreshTokens
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();
    }
}
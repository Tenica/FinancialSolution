using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class PasswordResetTokenRepository
    : IPasswordResetTokenRepository
{
    private readonly ApplicationDbContext _context;

    public PasswordResetTokenRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        PasswordResetToken token)
    {
        await _context.PasswordResetTokens
            .AddAsync(token);
    }

    public async Task<PasswordResetToken?>
        GetByTokenAsync(string token)
    {
        return await _context.PasswordResetTokens
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(
                x => x.Token == token);
    }
}
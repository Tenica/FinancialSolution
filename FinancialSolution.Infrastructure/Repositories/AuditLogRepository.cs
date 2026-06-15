using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialSolution.Infrastructure.Repositories;

public class AuditLogRepository
    : IAuditLogRepository
{
    private readonly ApplicationDbContext _context;

    public AuditLogRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        AuditLog auditLog)
    {
        await _context.AuditLogs.AddAsync(auditLog);
    }

    public async Task<IEnumerable<AuditLog>> GetAllAsync()
    {
        return await _context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(x => x.PerformedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>>
    GetByCustomerIdAsync(Guid customerId)
{
    return await _context.AuditLogs
        .AsNoTracking()
        .Where(x => x.CustomerId == customerId)
        .OrderByDescending(x => x.PerformedAt)
        .ToListAsync();
}
}
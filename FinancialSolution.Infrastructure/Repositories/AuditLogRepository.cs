using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;

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
}
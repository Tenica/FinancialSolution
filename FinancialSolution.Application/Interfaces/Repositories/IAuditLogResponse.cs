using FinancialSolution.Domain.Entities;

namespace FinancialSolution.Application.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog);
}
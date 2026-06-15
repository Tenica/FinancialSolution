using FinancialSolution.Domain.Entities;

namespace FinancialSolution.Application.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog);

    Task<IEnumerable<AuditLog>> GetAllAsync();

    Task<IEnumerable<AuditLog>> GetByCustomerIdAsync(
    Guid customerId);
}
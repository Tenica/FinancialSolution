using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Domain.Entities;

namespace FinancialSolution.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly IAuditLogRepository _auditRepository;

    private readonly IUnitOfWork _unitOfWork;

    public AuditService(
        IAuditLogRepository auditRepository,
        IUnitOfWork unitOfWork)
    {
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task LogAsync(
        Guid customerId,
        string action,
        string description,
        string? ipAddress)
    {
        var auditLog = new AuditLog
        {
            CustomerId = customerId,
            Action = action,
            Description = description,
            IpAddress = ipAddress,
            PerformedAt = DateTime.UtcNow
        };

        await _auditRepository.AddAsync(auditLog);

        await _unitOfWork.SaveChangesAsync();
    }
}
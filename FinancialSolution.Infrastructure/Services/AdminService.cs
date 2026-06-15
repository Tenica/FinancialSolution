using FinancialSolution.Application.DTOs.Admin;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Domain.Enums;
using FinancialSolution.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogRepository _auditLogRepository;

    public AdminService(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork, IAuditLogRepository auditLogRepository)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _auditLogRepository = auditLogRepository;
    }

    public async Task<IEnumerable<CustomerSummaryResponse>>
        GetCustomersAsync()
    {
        var customers =
            await _customerRepository.GetAllAsync();

        return customers.Select(customer =>
            new CustomerSummaryResponse
            {
                Id = customer.Id,

                FirstName = customer.FirstName,

                LastName = customer.LastName,

                Email = customer.Email,

                PhoneNumber = customer.PhoneNumber,

                Role = customer.Role.ToString(),

                IsActive = customer.IsActive,

                IsBvnVerified = customer.IsBvnVerified
            });
    }

    public async Task<CustomerDetailResponse>
     GetCustomerByIdAsync(Guid customerId)
    {
        var customer =
            await _customerRepository.GetByIdAsync(customerId);

        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        return new CustomerDetailResponse
        {
            Id = customer.Id,

            FirstName = customer.FirstName,

            LastName = customer.LastName,

            Email = customer.Email,

            PhoneNumber = customer.PhoneNumber,

            Role = customer.Role.ToString(),

            IsActive = customer.IsActive,

            BVN = customer.BVN,

            IsBvnVerified = customer.IsBvnVerified,

            BvnVerifiedAt = customer.BvnVerifiedAt
        };
    }

    public async Task DeactivateCustomerAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId) ?? throw new Exception("Customer not found.");
        customer.IsActive = false;

        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ActivateCustomerAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId) ?? throw new Exception("Customer not found.");
        customer.IsActive = true;

        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task PromoteToAdminAsync(Guid customerId)
    {
        var customer =
            await _customerRepository.GetByIdAsync(customerId) ?? throw new Exception("Customer not found.");
        customer.Role = UserRole.Admin;

        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DemoteToCustomerAsync(Guid customerId)
    {
        var customer =
            await _customerRepository.GetByIdAsync(customerId) ?? throw new Exception("Customer not found.");
        customer.Role = UserRole.Customer;

        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditLogResponse>>
    GetAuditLogsAsync()
    {
        var logs =
            await _auditLogRepository.GetAllAsync();

        return logs.Select(log =>
            new AuditLogResponse
            {
                Id = log.Id,

                CustomerId = log.CustomerId,

                Action = log.Action,

                Description = log.Description,

                PerformedAt = log.PerformedAt,

                IpAddress = log.IpAddress
            });
    }

    public async Task<IEnumerable<CustomerAuditLogResponse>>
    GetCustomerAuditLogsAsync(Guid customerId)
    {
        var customer =
            await _customerRepository.GetByIdAsync(customerId);

        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        var logs =
            await _auditLogRepository
                .GetByCustomerIdAsync(customerId);

        return logs.Select(log =>
            new CustomerAuditLogResponse
            {
                Id = log.Id,

                Action = log.Action,

                Description = log.Description,

                PerformedAt = log.PerformedAt,

                IpAddress = log.IpAddress
            });
    }
}

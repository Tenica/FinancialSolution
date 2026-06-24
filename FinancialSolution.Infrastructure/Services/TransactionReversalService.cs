using FinancialSolution.Application.DTOs.Reversal;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Domain.Enums;
using FinancialSolution.Infrastructure.Repositories;
using FinancialSolution.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Services
{
    public class TransactionReversalService : ITransactionReversalService

    {
        public readonly ITransactionReversalRepository _transactionReversalRepository;

        public readonly ITransactionRepository _transactionRepository;

        public readonly ICustomerRepository _customerRepository;

        public readonly IAuditService _auditService;

        public readonly IUnitOfWork _unitOfWork;

        public readonly INotificationService _notificationService;

        public TransactionReversalService(ITransactionReversalRepository transactionReversalRepository,
            ITransactionRepository transactionRepository, ICustomerRepository customerRepository, 
            IAuditService auditService, IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _transactionReversalRepository = transactionReversalRepository;

            _transactionRepository = transactionRepository;

            _customerRepository = customerRepository;

            _auditService = auditService;

            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }


        public async Task CreateAsync(Guid customerId, CreateReversalRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId) ?? throw new Exception(
                    "Customer not found.");
            var transaction = await _transactionRepository.GetByReferenceAsync(request.TransactionReference);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            var ExistingReversalRequest = await _transactionReversalRepository
        .ExistsForTransactionAsync(request.TransactionReference);

            if (ExistingReversalRequest)
            {
                throw new Exception(
                    "A reversal request already exists for this transaction.");
            }

            var reversalRequest = new TransactionReversalRequest
           {
             CustomerId = customerId,

             TransactionReference = request.TransactionReference,

               Reason = request.Reason
           };

            await _transactionReversalRepository.AddAsync(reversalRequest);

            await _unitOfWork.SaveChangesAsync();

            await _auditService.LogAsync(customerId,"Reversal Request",
            $"Reversal requested for transaction {request.TransactionReference}",
            null);

        }

        public async Task<List<ReversalResponse>> GetAsync(Guid customerId)
        {
            var requests = await _transactionReversalRepository.GetByCustomerIdAsync(customerId);

            return requests == null
                ? throw new Exception("Cannot find a request, contact support")
                : requests.Select(x => new ReversalResponse
        {
            Id = x.Id,

            TransactionReference =
                x.TransactionReference,

            Reason =
                x.Reason,

            Status =
                x.Status.ToString(),

            CreatedAt =
                x.CreatedAt
        })
    .ToList();
        }

        public async Task<List<ReversalResponse>>
            GetPendingAsync(Guid adminId)
        {

            var admin =
               await _customerRepository
                   .GetByIdAsync(adminId);

            if (admin == null)
            {
                throw new Exception(
                    "Admin not found.");
            }

            if (admin.Role != UserRole.Admin)
            {
                throw new Exception(
                    "Only administrators can get pending reversal requests.");
            }

            var requests =
                await _transactionReversalRepository
                    .GetPendingAsync();

            return requests
                .Select(x =>
                    new ReversalResponse
                    {
                        Id = x.Id,

                        TransactionReference =
                            x.TransactionReference,

                        Reason =
                            x.Reason,

                        Status =
                            x.Status.ToString(),

                        CreatedAt =
                            x.CreatedAt
                    })
                .ToList();
        }


        public async Task ApproveAsync(Guid adminId, Guid reversalRequestId)
        {
            var admin =
                await _customerRepository
                    .GetByIdAsync(adminId) ?? throw new Exception(
                    "Admin not found.");

            if (admin.Role != UserRole.Admin)
            {
                throw new Exception(
                    "Only administrators can approve reversal requests.");
            }

            var request =
                await _transactionReversalRepository
                    .GetByIdAsync(reversalRequestId) ?? throw new Exception(
                    "Reversal request not found.");


            if (request.Status != ReversalStatus.Pending)
            {
                throw new Exception(
                    "Only pending requests can be approved.");
            }

            request.Status =
                ReversalStatus.Approved;

            request.ReviewedAt =
                DateTime.UtcNow;

            request.ReviewedByAdminId =
                adminId;

            await _unitOfWork.SaveChangesAsync();

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer != null)
            {
                await _notificationService
                    .SendReversalApprovedNotificationAsync(
                        customer.Email,
                        request.TransactionReference);
            }

            await _auditService.LogAsync(
                adminId,
                "Approve Reversal",
                $"Approved reversal request {request.Id}",
                null);
        }


        public async Task RejectAsync(
    Guid adminId,
    Guid reversalRequestId,
    string reason)
        {
            var admin =
                await _customerRepository
                    .GetByIdAsync(adminId) ?? throw new Exception(
                    "Admin not found.");
              

            if (admin.Role != UserRole.Admin)
            {
                throw new Exception(
                    "Only administrators can reject reversal requests.");
            }

            var request =
                await _transactionReversalRepository
                    .GetByIdAsync(reversalRequestId) ?? 
                    throw new Exception("Reversal request not found.");

            if (request.Status != ReversalStatus.Pending)
            {
                throw new Exception(
                    "Only pending requests can be rejected.");
            }

            request.Status =
                ReversalStatus.Rejected;

            request.ReviewedAt =
                DateTime.UtcNow;

            request.ReviewedByAdminId =
                adminId;

            await _unitOfWork.SaveChangesAsync();

            var customer =await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer != null)
            {
                await _notificationService
                    .SendReversalRejectedNotificationAsync(
                        customer.Email,
                        request.TransactionReference, reason);
            }

            await _auditService.LogAsync(
                adminId,
                "Reject Reversal",
                $"Rejected reversal request {request.Id}. Reason: {reason}",
                null);
        }
    }
}

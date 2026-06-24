using FinancialSolution.Application.DTOs.ScheduledTransfer;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Services
{
    public class ScheduledTransferService
     : IScheduledTransferService
    {
        private readonly IWalletRepository _walletRepository;

        private readonly IScheduledTransferRepository
            _scheduledTransferRepository;

        private readonly IUnitOfWork _unitOfWork;

        public ScheduledTransferService(
           IScheduledTransferRepository scheduledTransferRepository,
           IUnitOfWork unitOfWork,
           IWalletRepository walletRepository)
        {
            _scheduledTransferRepository =
                scheduledTransferRepository;

            _unitOfWork = unitOfWork;
            _walletRepository = walletRepository;
        }

        public async Task<ScheduledTransferResponse>
     CreateAsync(Guid customerId, CreateScheduledTransfer request)

        {
            var wallet =
    await _walletRepository
        .GetByCustomerIdAsync(customerId) ?? throw new Exception("Wallet not found.");

            if (wallet.AccountNumber
                != request.SenderAccountNumber)
            {
                throw new Exception(
                    "You can only schedule transfers from your own account.");
            }


            if (request.NextExecutionDate <= DateTime.UtcNow)
            {
                throw new Exception(
                    "Execution date must be in the future.");

           
            }

            var scheduledTransfer =
                new ScheduledTransfer
                {
                    CustomerId = customerId,

                    SenderAccountNumber =
                        request.SenderAccountNumber,

                    ReceiverAccountNumber =
                        request.ReceiverAccountNumber,

                    Amount = request.Amount,

                    NextExecutionDate =
                        request.NextExecutionDate,

                    RecurrenceType =
                        request.RecurrenceType,

                    IsActive = true
                };

            await _scheduledTransferRepository
                .AddAsync(scheduledTransfer);

            await _unitOfWork.SaveChangesAsync();

            return new ScheduledTransferResponse
            {
                Id = scheduledTransfer.Id,

                SenderAccountNumber =
                    scheduledTransfer.SenderAccountNumber,

                ReceiverAccountNumber =
                    scheduledTransfer.ReceiverAccountNumber,

                Amount =
                    scheduledTransfer.Amount,

                NextExecutionDate =
                    scheduledTransfer.NextExecutionDate,

                RecurrenceType =
                    scheduledTransfer
                        .RecurrenceType
                        .ToString(),

                IsActive =
                    scheduledTransfer.IsActive
            };
        }

        public async Task<List<ScheduledTransferResponse>>
     GetAsync(Guid customerId)
        {
            var transfers =
                await _scheduledTransferRepository
                    .GetByCustomerIdAsync(customerId);

            return [.. transfers
                .Select(x =>
                    new ScheduledTransferResponse
                    {
                        Id = x.Id,

                        SenderAccountNumber =
                            x.SenderAccountNumber,

                        ReceiverAccountNumber =
                            x.ReceiverAccountNumber,

                        Amount = x.Amount,

                        NextExecutionDate =
                            x.NextExecutionDate,

                        RecurrenceType =
                            x.RecurrenceType
                                .ToString(),

                        IsActive =
                            x.IsActive
                    })];
        }

        public async Task PauseAsync(
     Guid customerId,
     Guid scheduledTransferId)
        {
            var transfer =
                await _scheduledTransferRepository
                    .GetByIdAsync(scheduledTransferId) ?? throw new Exception(
                    "Scheduled transfer not found.");

            if (transfer.CustomerId != customerId)
            {
                throw new Exception(
                    "You do not own this scheduled transfer.");
            }

            transfer.IsActive = false;

            await _unitOfWork.SaveChangesAsync();
        }



        public async Task ResumeAsync(
     Guid customerId,
     Guid scheduledTransferId)
        {
            var transfer =
                await _scheduledTransferRepository
                    .GetByIdAsync(scheduledTransferId) ?? throw new Exception(
                    "Scheduled transfer not found.");

            if (transfer.CustomerId != customerId)
            {
                throw new Exception(
                    "You do not own this scheduled transfer.");
            }

            transfer.IsActive = true;

            await _unitOfWork.SaveChangesAsync();
        }



        public async Task CancelAsync(
      Guid customerId,
      Guid scheduledTransferId)
        {
            var transfer =
                await _scheduledTransferRepository
                    .GetByIdAsync(scheduledTransferId);

            if (transfer == null)
            {
                throw new Exception(
                    "Scheduled transfer not found.");
            }

            if (transfer.CustomerId != customerId)
            {
                throw new Exception(
                    "You do not own this scheduled transfer.");
            }

            transfer.IsActive = false;

            await _unitOfWork.SaveChangesAsync();
        }


    }
}
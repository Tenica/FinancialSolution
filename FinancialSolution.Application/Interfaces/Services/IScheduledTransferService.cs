using FinancialSolution.Application.DTOs.ScheduledTransfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.Interfaces.Services
{
    public interface IScheduledTransferService
    {
        Task<ScheduledTransferResponse> CreateAsync(
            Guid customerId,
            CreateScheduledTransfer request);

        Task<List<ScheduledTransferResponse>> GetAsync(
            Guid customerId);

        Task PauseAsync(
            Guid customerId,
            Guid scheduledTransferId);

        Task ResumeAsync(
            Guid customerId,
            Guid scheduledTransferId);

        Task CancelAsync(
            Guid customerId,
            Guid scheduledTransferId);
    }
}

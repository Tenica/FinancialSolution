using FinancialSolution.Application.DTOs.Reversal;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.Interfaces.Services
{
    public interface ITransactionReversalService
    {
        Task CreateAsync(
        Guid customerId,
        CreateReversalRequest request);

        Task<List<ReversalResponse>>
            GetAsync(Guid customerId);


        Task ApproveAsync(
        Guid adminId,
        Guid reversalRequestId);

        Task RejectAsync(
            Guid adminId,
            Guid reversalRequestId,
            string reason);

        Task<List<ReversalResponse>>
            GetPendingAsync(Guid adminId);
    }
}

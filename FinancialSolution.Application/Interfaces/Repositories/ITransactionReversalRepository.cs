using FinancialSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.Interfaces.Repositories
{
    public interface ITransactionReversalRepository
    : IGenericRepository<TransactionReversalRequest>
    {
        Task<List<TransactionReversalRequest>>
            GetByCustomerIdAsync(Guid customerId);

        Task<List<TransactionReversalRequest>>GetPendingAsync();

        Task<bool>
            ExistsForTransactionAsync(
                string transactionReference);
    }
}

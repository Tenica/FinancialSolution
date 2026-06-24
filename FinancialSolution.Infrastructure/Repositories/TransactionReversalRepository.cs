using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Domain.Enums;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Repositories
{
    public class TransactionReversalRepository
    : GenericRepository<TransactionReversalRequest>,
      ITransactionReversalRepository
    {
        public TransactionReversalRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<List<TransactionReversalRequest>>
            GetByCustomerIdAsync(Guid customerId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TransactionReversalRequest>>
    GetPendingAsync()
        {
            return await _dbSet
                .Where(x =>
                    x.Status ==
                    ReversalStatus.Pending)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool>
            ExistsForTransactionAsync(
                string transactionReference)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(x =>
                    x.TransactionReference ==
                    transactionReference);
        }
    }
}

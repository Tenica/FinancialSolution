using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Repositories;

public class BeneficiaryRepository
    : GenericRepository<Beneficiary>,
      IBeneficiaryRepository
{
    public BeneficiaryRepository(
        ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<Beneficiary>>
        GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            //Read-only query No updates required hence .AsNoTracking
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderBy(x => x.BeneficiaryName)
            .ToListAsync();
    }
    public async Task<bool> ExistsAsync(Guid customerId, string accountNumber, string bankName)
    {
        return await _dbSet
        .AsNoTracking()
        .AnyAsync(x =>
            x.CustomerId == customerId &&
            x.AccountNumber == accountNumber &&
            x.BankName == bankName);
    }

}

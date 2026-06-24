using System;
using System.Collections.Generic;
using System.Text;

using FinancialSolution.Domain.Entities;

namespace FinancialSolution.Application.Interfaces.Repositories;

public interface IBeneficiaryRepository
    : IGenericRepository<Beneficiary>
{
    Task<List<Beneficiary>>
        GetByCustomerIdAsync(Guid customerId);

    Task<bool> ExistsAsync(
    Guid customerId,
    string accountNumber,
    string bankName);
}
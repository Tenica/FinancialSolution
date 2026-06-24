using System;
using System.Collections.Generic;
using System.Text;

using FinancialSolution.Application.DTOs.Beneficiary;

namespace FinancialSolution.Application.Interfaces.Services;

public interface IBeneficiaryService
{
    Task CreateAsync(
        Guid customerId,
        CreateBeneficiaryRequest request);

   

    Task<List<BeneficiaryResponse>>
        GetAsync(Guid customerId);

    Task DeleteAsync(
        Guid customerId,
        Guid beneficiaryId);
}

using FinancialSolution.Application.DTOs.Kyc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.Interfaces.Services
{
    public interface IKycService
    {
        Task<VerifyBvnResponse> VerifyBvnAsync(
      Guid customerId,
      VerifyBvnRequest request);
    }

}

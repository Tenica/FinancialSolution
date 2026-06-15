using FinancialSolution.Application.DTOs.Kyc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.Interfaces.Services;

public interface IDojahService
{
    Task<BvnVerificationResult> VerifyBvnAsync(
    string bvn,
    string firstName,
    string lastName);
}
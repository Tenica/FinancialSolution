using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Kyc;

public class VerifyBvnRequest
{
    public string Bvn { get; set; } = default!;
}
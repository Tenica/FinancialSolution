using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Kyc
{
    public class VerifyBvnResponse
    {
        public string Message { get; set; } = default!;

        public bool IsBvnVerified { get; set; }
    }
}

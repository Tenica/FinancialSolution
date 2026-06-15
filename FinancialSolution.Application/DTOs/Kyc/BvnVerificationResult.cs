using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Kyc
{
    public class BvnVerificationResult
    {
        public bool IsVerified { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ErrorMessage { get; set; }
    }
}

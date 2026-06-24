using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Reversal
{
    public class CreateReversalRequest
    {
        public string TransactionReference
        { get; set; } = default!;

        public string Reason
        { get; set; } = default!;
    }
}

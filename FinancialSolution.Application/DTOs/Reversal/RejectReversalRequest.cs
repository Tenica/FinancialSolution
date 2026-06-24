using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Reversal
{
    public class RejectReversalRequest
    {
        public string Reason
        { get; set; } = default!;
    }
}

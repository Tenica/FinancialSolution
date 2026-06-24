using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Reversal
{
    public class ReversalResponse
    {
        public Guid Id { get; set; }

        public string TransactionReference
        { get; set; } = default!;

        public string Reason
        { get; set; } = default!;

        public string Status
        { get; set; } = default!;

        public DateTime CreatedAt
        { get; set; }
    }
}

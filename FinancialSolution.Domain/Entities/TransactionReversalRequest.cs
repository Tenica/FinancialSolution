using FinancialSolution.Domain.Common;
using FinancialSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Domain.Entities
{
    public class TransactionReversalRequest : BaseEntity
    {
        public Guid CustomerId { get; set; }

        public string TransactionReference { get; set; } = default!;

        public string Reason { get; set; } = default!;

        public ReversalStatus Status { get; set; }
            = ReversalStatus.Pending;

        public DateTime? ReviewedAt { get; set; }

        public Guid? ReviewedByAdminId { get; set; }


        // Navigation
        public Customer Customer { get; set; }
            = default!;
    }

}
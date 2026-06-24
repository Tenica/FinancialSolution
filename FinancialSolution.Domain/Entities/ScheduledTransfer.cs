using FinancialSolution.Domain.Common;
using FinancialSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Domain.Entities
{
    public class ScheduledTransfer : BaseEntity
    {
        public Guid CustomerId { get; set; }

        public string SenderAccountNumber { get; set; } = default!;

        public string ReceiverAccountNumber { get; set; } = default!;

        public decimal Amount { get; set; }

        public DateTime NextExecutionDate { get; set; }

        public RecurrenceType RecurrenceType { get; set; }

        public string? LastTransactionReference { get; set; }

        public bool IsActive { get; set; } = true;

        public int FailedAttemptCount { get; set; }

        public DateTime? LastExecutedAt { get; set; }

        public Customer Customer { get; set; } = default!;
    }
}

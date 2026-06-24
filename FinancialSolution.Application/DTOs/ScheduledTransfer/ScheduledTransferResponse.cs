using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.ScheduledTransfer
{
    public class ScheduledTransferResponse
    {
        public Guid Id { get; set; }

        public string SenderAccountNumber { get; set; } = default!;

        public string ReceiverAccountNumber { get; set; } = default!;

        public decimal Amount { get; set; }

        public DateTime NextExecutionDate { get; set; }

        public string RecurrenceType { get; set; } = default!;

        public bool IsActive { get; set; }
    }
}

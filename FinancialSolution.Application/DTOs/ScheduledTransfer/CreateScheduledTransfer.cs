using FinancialSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.ScheduledTransfer
{
    public class CreateScheduledTransfer
    {
        public string SenderAccountNumber { get; set; } = default!;

        public string ReceiverAccountNumber { get; set; } = default!;

        public decimal Amount { get; set; }

        public DateTime NextExecutionDate { get; set; }

        public RecurrenceType RecurrenceType { get; set; }
    }
}

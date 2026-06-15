using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Admin
{


    public class CustomerAuditLogResponse
    {
        public Guid Id { get; set; }

        public string Action { get; set; } = default!;

        public string Description { get; set; } = default!;

        public DateTime PerformedAt { get; set; }

        public string? IpAddress { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.ExternalServices.Dojah.DTOs
{
    public class DojahValidationResult
    {
        public string Value { get; set; } = default!;

        public bool Status { get; set; }
    }
}

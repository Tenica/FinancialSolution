using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FinancialSolution.Infrastructure.ExternalServices.Dojah.DTOs
{
    public class DojahMatchResult
    {
        [JsonPropertyName("confidence_value")]
        public int ConfidenceValue { get; set; }

        public bool Status { get; set; }
    }
}

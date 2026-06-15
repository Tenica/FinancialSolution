using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FinancialSolution.Infrastructure.ExternalServices.Dojah.DTOs;

public class DojahEntity
{
    public DojahValidationResult Bvn { get; set; } = default!;

    [JsonPropertyName("first_name")]
    public DojahMatchResult FirstName { get; set; } = default!;

    [JsonPropertyName("last_name")]
    public DojahMatchResult LastName { get; set; } = default!;
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.DTOs.Beneficiary;

public class BeneficiaryResponse
{
    public Guid Id { get; set; }

    public string BeneficiaryName { get; set; } = default!;

    public string AccountNumber { get; set; } = default!;

    public string BankName { get; set; } = default!;
}

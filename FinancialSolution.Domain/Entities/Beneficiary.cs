using FinancialSolution.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Domain.Entities;

    public class Beneficiary : BaseEntity
{
    public Guid CustomerId { get; set; }

    public string BeneficiaryName { get; set; } = default!;

    public string AccountNumber { get; set; } = default!;

    public string BankName { get; set; } = default!;

    // Navigation Property
    public Customer Customer { get; set; } = default!;
}

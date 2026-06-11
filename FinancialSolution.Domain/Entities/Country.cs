using FinancialSolution.Domain.Common;

namespace FinancialSolution.Domain.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = default!;

    public string IsoCode { get; set; } = default!;

    public Guid CurrencyId { get; set; }

    // Navigation Property
    public Currency Currency { get; set; } = default!;
}
using FinancialSolution.Domain.Common;

namespace FinancialSolution.Domain.Entities;

public class Currency : BaseEntity
{
    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Symbol { get; set; } = default!;

    public int DecimalPlaces { get; set; }
}
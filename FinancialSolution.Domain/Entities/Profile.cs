using FinancialSolution.Domain.Common;

namespace FinancialSolution.Domain.Entities;

public class Profile : BaseEntity
{
    public Guid CustomerId { get; set; }

    public string Address { get; set; } = default!;

    public DateTime DateOfBirth { get; set; }

    public Guid CountryId { get; set; }

    // Navigation Properties
    public Customer Customer { get; set; } = default!;

    public Country Country { get; set; } = default!;
}
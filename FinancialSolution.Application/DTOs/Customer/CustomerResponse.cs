namespace FinancialSolution.Application.DTOs.Customer;

public class CustomerResponse
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string AccountNumber { get; set; } = default!;

    public string CurrencyCode { get; set; } = default!;
}
namespace FinancialSolution.Application.DTOs.Customer;

public class RegisterCustomerRequest
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string BVN { get; set; } = default!;

    public string Password { get; set; } = default!;

    public Guid CountryId { get; set; }
}
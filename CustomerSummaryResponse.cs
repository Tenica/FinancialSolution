public class CustomerSummaryResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string Role { get; set; } = default!;

    public bool IsActive { get; set; }

    public bool IsBvnVerified { get; set; }
}
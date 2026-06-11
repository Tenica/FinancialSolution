namespace FinancialSolution.Application.DTOs.Authentication;

public class LoginResponse
{
    public string Token { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public DateTime Expiration { get; set; }

    public string Email { get; set; } = default!;

    public string Role { get; set; } = default!;
}
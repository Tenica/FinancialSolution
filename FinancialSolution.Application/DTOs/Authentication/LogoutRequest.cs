namespace FinancialSolution.Application.DTOs.Authentication;

public class LogoutRequest
{
    public string RefreshToken { get; set; } = default!;
}
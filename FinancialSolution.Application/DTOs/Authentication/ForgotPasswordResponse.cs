namespace FinancialSolution.Application.DTOs.Authentication;

public class ForgotPasswordResponse
{
    public string Message { get; set; } = default!;

    public string ResetToken { get; set; } = default!;
}
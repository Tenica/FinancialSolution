using FinancialSolution.Application.DTOs.Authentication;

namespace FinancialSolution.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);

    Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);

    Task LogoutAsync(LogoutRequest request);

    Task<ForgotPasswordResponse> ForgotPasswordAsync(
    ForgotPasswordRequest request);

    Task<ResetPasswordResponse> ResetPasswordAsync(string token, string newPassword);
}
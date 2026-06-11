using FinancialSolution.Application.DTOs.Authentication;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    private readonly IEmailService _emailService;

    public AuthenticationController(
        IAuthenticationService authenticationService, IEmailService emailService)
    {
        _authenticationService = authenticationService;

        _emailService = emailService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response =
            await _authenticationService.LoginAsync(request);

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
    RefreshTokenRequest request)
    {
        var response =
            await _authenticationService
                .RefreshTokenAsync(request);

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    [FromBody] LogoutRequest request)
    {
        await _authenticationService
            .LogoutAsync(request);

        return Ok(new
        {
            Message = "Logged out successfully."
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult>
    ForgotPassword(
        ForgotPasswordRequest request)
    {
        var response =
            await _authenticationService
                .ForgotPasswordAsync(request);

        return Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult>
    ResetPassword([FromQuery] string token,
    [FromBody] ResetPasswordRequest request)
    {
        var response =
            await _authenticationService
                .ResetPasswordAsync(token, request.NewPassword);

        return Ok(response);
    }

    [HttpPost("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        await _emailService.SendAsync(
            "mmaduabuchichisom99@gmail.com",
            "FinancialSolution Test",
            "<h2>Email Test Successful</h2><p>Your email integration works!</p>");

        return Ok(new
        {
            Message = "Email sent successfully."
        });
    }
}
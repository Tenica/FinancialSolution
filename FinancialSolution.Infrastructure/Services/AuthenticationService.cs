using BCrypt.Net;
using FinancialSolution.Application.DTOs.Authentication;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinancialSolution.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IConfiguration _configuration;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IEmailService _emailService;

    private readonly IPasswordResetTokenRepository
    _passwordResetTokenRepository;

    public AuthenticationService(
      ICustomerRepository customerRepository,
      IConfiguration configuration,
      IRefreshTokenRepository refreshTokenRepository,
      IPasswordResetTokenRepository passwordResetTokenRepository,
      IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _customerRepository = customerRepository;

        _configuration = configuration;
      
        _refreshTokenRepository = refreshTokenRepository;

        _passwordResetTokenRepository = passwordResetTokenRepository;

        _unitOfWork = unitOfWork;

        _emailService = emailService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var customer =
            await _customerRepository.GetByEmailAsync(request.Email);

        if (customer == null)
        {
            throw new Exception("Invalid email or password.");
        }

        if (!customer.IsActive)
        {
            throw new Exception(
                "Your account has been deactivated. Please contact support.");
        }

        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                customer.PasswordHash);

        if (!passwordValid)
        {
            throw new Exception("Invalid email or password.");
        }

        var token = GenerateJwtToken(customer.Id.ToString(), customer.Email, customer.Role.ToString());
        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity =
    new RefreshToken
    {
        CustomerId = customer.Id,

        Token = refreshToken,

        ExpiresAt =
            DateTime.UtcNow.AddDays(7),

        IsRevoked = false
    };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        await _unitOfWork.SaveChangesAsync();

        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            Email = customer.Email,
            Role = customer.Role.ToString()
        };
    }

    public async Task<LoginResponse> RefreshTokenAsync(
    RefreshTokenRequest request)
    {
        var existingToken =
            await _refreshTokenRepository
                .GetByTokenAsync(request.RefreshToken);

        if (existingToken == null)
        {
            throw new Exception("Invalid refresh token.");
        }

        if (existingToken.IsRevoked)
        {
            throw new Exception("Refresh token has been revoked.");
        }

        if (existingToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new Exception("Refresh token has expired.");
        }

        var customer = existingToken.Customer;

        var accessToken = GenerateJwtToken(
            customer.Id.ToString(),
            customer.Email,
            customer.Role.ToString());

        var newRefreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            CustomerId = customer.Id,

            Token = newRefreshToken,

            ExpiresAt = DateTime.UtcNow.AddDays(7),

            IsRevoked = false
        };

        existingToken.IsRevoked = true;

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        await _unitOfWork.SaveChangesAsync();

        return new LoginResponse
        {
            Token = accessToken,

            RefreshToken = newRefreshToken,

            Expiration = DateTime.UtcNow.AddMinutes(60),

            Email = customer.Email,

            Role = customer.Role.ToString()
        };
    }

   

 

private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    private string GenerateJwtToken(string customerId, string email, string role)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var secretKey = jwtSettings["SecretKey"];

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
    new Claim(ClaimTypes.NameIdentifier, customerId),

    new Claim(JwtRegisteredClaimNames.Sub, customerId),

    new Claim(JwtRegisteredClaimNames.Email, email),

    new Claim(
    System.Security.Claims.ClaimTypes.Role, role),

    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task LogoutAsync(
        LogoutRequest request)
    {
        var refreshToken =
            await _refreshTokenRepository
                .GetByTokenAsync(request.RefreshToken);

        if (refreshToken == null)
        {
            throw new Exception(
                "Invalid refresh token.");
        }

        if (refreshToken.IsRevoked)
        {
            throw new Exception(
                "Refresh token has already been revoked.");
        }

        refreshToken.IsRevoked = true;

        await _unitOfWork.SaveChangesAsync();
    }


    public async Task<ForgotPasswordResponse>
    ForgotPasswordAsync(
        ForgotPasswordRequest request)
    {
        var customer =
            await _customerRepository
                .GetByEmailAsync(request.Email);

        if (customer == null)
        {
            throw new Exception(
                "Customer not found.");
        }

        var resetToken =
            Guid.NewGuid().ToString();

        var passwordResetToken =
            new PasswordResetToken
            {
                CustomerId = customer.Id,

                Token = resetToken,

                ExpiresAt =
                    DateTime.UtcNow.AddHours(1),

                IsUsed = false
            };

        await _passwordResetTokenRepository
            .AddAsync(passwordResetToken);

        await _unitOfWork
            .SaveChangesAsync();

        var resetLink =
    $"https://localhost:3000/reset-password?token={resetToken}";

        await _emailService.SendAsync(
            customer.Email,
            "Reset Your Password",
            $@"
        <h2>Password Reset Request</h2>

        <p>Click the link below to reset your password:</p>

        <a href='{resetLink}'>
            Reset Password
        </a>

        <p>This link expires in 1 hour.</p>
    ");

        return new ForgotPasswordResponse
        {
            Message =
                "If an account exists, a password reset email has been sent.",

        };
    }


    public async Task<ResetPasswordResponse>
        ResetPasswordAsync(string token, string newPassword)
    {
        var passwordResetToken =
            await _passwordResetTokenRepository
                .GetByTokenAsync(token);
        if (passwordResetToken == null)
        {
            throw new Exception(
                "Invalid reset token.");
        }

        if (passwordResetToken.IsUsed)
        {
            throw new Exception(
                "Reset token has already been used.");
        }

        if (passwordResetToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new Exception(
                "Reset token has expired.");
        }

        var customer = passwordResetToken.Customer;

        customer.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(newPassword);

        passwordResetToken.IsUsed = true;

        var refreshTokens =
            await _refreshTokenRepository
                .GetByCustomerIdAsync(customer.Id);

        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.IsRevoked = true;
        }

        await _unitOfWork.SaveChangesAsync();

        return new ResetPasswordResponse
        {
            Message =
                "Password reset successfully."
        };
    }

}
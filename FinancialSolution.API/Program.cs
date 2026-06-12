using Microsoft.EntityFrameworkCore;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Application.Services;
using FinancialSolution.Infrastructure.Services;
using FinancialSolution.Infrastructure.Persistence;
using FinancialSolution.Infrastructure.Repositories;
using FinancialSolution.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinancialSolution.API.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using FinancialSolution.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings =
    builder.Configuration.GetSection("JwtSettings");

var secretKey = jwtSettings["SecretKey"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],

                ValidAudience = jwtSettings["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey!))
            };
    });

// 1. Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Dependency Injection

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<ICountryRepository, CountryRepository>();

builder.Services.AddScoped<IWalletRepository, WalletRepository>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IWalletService, WalletService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddScoped<IAccountNumberGenerator, AccountNumberGenerator>();

builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();



builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

builder.Services.AddScoped<IKycService,KycService>();






// 3. API Configuration
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<
    RegisterCustomerRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // The classic Swagger generator

var app = builder.Build();

// 4. HTTP Pipeline
//app.UseHttpsRedirection();

// The classic Swagger visual UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();
// 👇 ADD THESE TWO LINES RIGHT HERE 👇
app.UseAuthentication(); // 1. "Who are you?" (Checks the JWT)
app.UseAuthorization();  // 2. "Are you allowed in?" (Checks permissions)

// The classic way to map controllers
app.MapControllers();

app.Run();
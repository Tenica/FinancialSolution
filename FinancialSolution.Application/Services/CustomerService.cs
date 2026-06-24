using BCrypt.Net;
using FinancialSolution.Application.DTOs.Customer;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Domain.Enums;

namespace FinancialSolution.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    private readonly ICountryRepository _countryRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IWalletRepository _walletRepository;

    private readonly IAccountNumberGenerator _accountNumberGenerator;

    public CustomerService(
        ICustomerRepository customerRepository,
        ICountryRepository countryRepository,
        IUnitOfWork unitOfWork,
        IWalletRepository walletRepository,
        IAccountNumberGenerator accountNumberGenerator)
    {
        _customerRepository = customerRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _walletRepository = walletRepository;
        _accountNumberGenerator = accountNumberGenerator;
    }

    public async Task<CustomerResponse> RegisterCustomerAsync(
    RegisterCustomerRequest request)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var existingEmail =
                await _customerRepository.GetByEmailAsync(request.Email);

            if (existingEmail != null)
            {
                throw new Exception("Email already exists.");
            }

            var existingBvn =
                await _customerRepository.GetByBvnAsync(request.BVN);

            if (existingBvn != null)
            {
                throw new Exception("BVN already exists.");
            }

            var country =
                await _countryRepository
                    .GetCountryWithCurrencyAsync(request.CountryId);

            if (country == null)
            {
                throw new Exception("Country not found.");
            }

            var passwordHash =
                BCrypt.Net.BCrypt.HashPassword(request.Password);

            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                BVN = request.BVN,
                PasswordHash = passwordHash,
                IsBvnVerified = false,
                Role = UserRole.Customer
            };

            await _customerRepository.AddAsync(customer);

            await _unitOfWork.SaveChangesAsync();

            var accountNumber =
                _accountNumberGenerator.Generate(
                    country.Currency.Code);

            await _walletRepository.CreateWalletAsync(
                customer.Id,
                country.CurrencyId,
                accountNumber);

            await _unitOfWork.CommitTransactionAsync();

            return new CustomerResponse
            {
                Id = customer.Id,

                FullName =
                    $"{customer.FirstName} {customer.LastName}",

                Email = customer.Email,

                AccountNumber = accountNumber,

                CurrencyCode = country.Currency.Code
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();

            throw;
        }
    }

    public async Task<CustomerResponse?> GetCustomerByEmailAsync(string email)
    {
        var customer = await _customerRepository.GetByEmailAsync(email);

        if (customer == null)
        {
            return null;
        }

        return new CustomerResponse
        {
            Id = customer.Id,
            FullName = $"{customer.FirstName} {customer.LastName}",
            Email = customer.Email,
           
        };
    }
}




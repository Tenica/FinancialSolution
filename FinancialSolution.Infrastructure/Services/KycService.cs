using System;
using System.Collections.Generic;
using System.Text;
using FinancialSolution.Application.DTOs.Kyc;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;

namespace FinancialSolution.Infrastructure.Services;

public class KycService : IKycService
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IUnitOfWork _unitOfWork;

    public KycService(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;

        _unitOfWork = unitOfWork;
    }

    public async Task<VerifyBvnResponse> VerifyBvnAsync(
        Guid customerId,
        VerifyBvnRequest request)
    {
        var customer =
            await _customerRepository.GetByIdAsync(customerId);

        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        if (customer.IsBvnVerified)
        {
            throw new Exception("BVN has already been verified.");
        }

        if (string.IsNullOrWhiteSpace(request.Bvn))
        {
            throw new Exception("BVN is required.");
        }

        if (request.Bvn.Length != 11 ||
            !request.Bvn.All(char.IsDigit))
        {
            throw new Exception(
                "BVN must contain exactly 11 digits.");
        }

        /*
         * SIMULATED BVN VERIFICATION
         */

        var verificationSucceeded = true;

        if (!verificationSucceeded)
        {
            throw new Exception(
                "BVN verification failed.");
        }

        customer.BVN = request.Bvn;

        customer.IsBvnVerified = true;

        customer.BvnVerifiedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return new VerifyBvnResponse
        {
            Message = "BVN verified successfully.",

            IsBvnVerified = true
        };
    }
}
using FinancialSolution.Application.DTOs.Authentication;
using FinancialSolution.Application.DTOs.Beneficiary;
using FinancialSolution.Application.DTOs.Common;
using FinancialSolution.Application.DTOs.Transaction;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IBeneficiaryRepository
    _beneficiaryRepository;

        private readonly IUnitOfWork
            _unitOfWork;

        public BeneficiaryService(
            IBeneficiaryRepository beneficiaryRepository,
            IUnitOfWork unitOfWork)
        {
            _beneficiaryRepository =beneficiaryRepository;

            _unitOfWork = unitOfWork;
        }

       

          public async Task CreateAsync(Guid customerId, CreateBeneficiaryRequest request) {

            var existingBeneficiary = await _beneficiaryRepository.ExistsAsync(customerId, request.AccountNumber, request.BankName);

            if (existingBeneficiary)
            {
                throw new Exception(
                    "Beneficiary already exists.");
            }

               var beneficiary = new Beneficiary {
               CustomerId = customerId,

               BeneficiaryName = request.BeneficiaryName,

               AccountNumber = request.AccountNumber,

               BankName = request.BankName
              };

            await _beneficiaryRepository.AddAsync(beneficiary);

            await _unitOfWork.SaveChangesAsync();
        }

       

        public async Task<List<BeneficiaryResponse>> GetAsync(Guid customerId)
        {
            var beneficiaries = await _beneficiaryRepository.GetByCustomerIdAsync(customerId);

            return [.. beneficiaries.Select(x =>
                  new BeneficiaryResponse
                  {
                      Id = x.Id,
                      BeneficiaryName = x.BeneficiaryName,
                      AccountNumber = x.AccountNumber,
                      BankName = x.BankName
                  })];
        }

       public async Task DeleteAsync(Guid customerId, Guid beneficiaryId)
        {
            var beneficiary = await _beneficiaryRepository.GetByIdAsync(beneficiaryId) ?? throw new Exception(
                    "Beneficiary not found.");

            if (beneficiary.CustomerId != customerId)
            {
                throw new Exception(
                    "Unauthorized.");
            }

            _beneficiaryRepository.Delete(beneficiary);

            await _unitOfWork
                .SaveChangesAsync();
        }
    }
}

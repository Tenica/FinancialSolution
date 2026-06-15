using FinancialSolution.Application.DTOs.Kyc;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FinancialSolution.Infrastructure.ExternalServices.Dojah
{
    public class DojahService : IDojahService
    {

        private readonly HttpClient _httpClient;

        private readonly DojahSettings _dojahSettings;

        public DojahService(
    HttpClient httpClient,
    IOptions<DojahSettings> dojahOptions)
        {
            _httpClient = httpClient;

            _dojahSettings = dojahOptions.Value;
        }

        public Task<BvnVerificationResult> VerifyBvnAsync(
            string bvn,
            string firstName,
            string lastName)
        {
            throw new NotImplementedException();
        }
    }
}

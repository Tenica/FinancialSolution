using FinancialSolution.Application.DTOs.Kyc;
using FinancialSolution.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class KycController : ControllerBase
{
    private readonly IKycService _kycService;

    public KycController(IKycService kycService)
    {
        _kycService = kycService;
    }

    [HttpPost("verify-bvn")]
    public async Task<IActionResult> VerifyBvn(
        VerifyBvnRequest request)
    {
        var customerId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(customerId))
        {
            return Unauthorized();
        }

        var response =
            await _kycService.VerifyBvnAsync(
                Guid.Parse(customerId),
                request);

        return Ok(response);
    }
}
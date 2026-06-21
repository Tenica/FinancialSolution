using FinancialSolution.Application.DTOs.Beneficiary;
using FinancialSolution.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BeneficiaryController : ControllerBase
{
    private readonly IBeneficiaryService _beneficiaryService;

    public BeneficiaryController(
        IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateBeneficiaryRequest request)
    {
        var customerId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(customerId))
        {
            return Unauthorized();
        }

        await _beneficiaryService.CreateAsync(
            Guid.Parse(customerId),
            request);

        return Ok(new
        {
            Message = "Beneficiary added successfully."
        });
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var customerId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(customerId))
        {
            return Unauthorized();
        }

        var beneficiaries =
            await _beneficiaryService.GetAsync(
                Guid.Parse(customerId));

        return Ok(beneficiaries);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var customerId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(customerId))
        {
            return Unauthorized();
        }

        await _beneficiaryService.DeleteAsync(
            Guid.Parse(customerId),
            id);

        return Ok(new
        {
            Message = "Beneficiary deleted successfully."
        });
    }
}
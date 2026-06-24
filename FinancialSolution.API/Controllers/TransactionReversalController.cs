using FinancialSolution.Application.DTOs.Reversal;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionReversalController
    : ControllerBase
{
    private readonly
        ITransactionReversalService
        _transactionReversalService;

    public TransactionReversalController(
        ITransactionReversalService
            transactionReversalService)
    {
        _transactionReversalService =
            transactionReversalService;
    }

    [HttpPost]
    // ARCHITECT'S NOTE: IActionResult
    // We return IActionResult instead of a specific object because it gives us the power 
    // to return different HTTP status codes based on the situation. 
    // It allows us to return 'Unauthorized()' (HTTP 401) if they fail the security check, 
    // or 'Ok()' (HTTP 200) if they succeed, all from the same method.
    public async Task<IActionResult> Create([FromBody] CreateReversalRequest request)
    {
        var customerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // ARCHITECT'S NOTE: Guid.TryParse vs Guid.Parse
        // We NEVER use Guid.Parse() on data coming from the internet. 
        // If the JWT token is corrupted or tampered with (e.g., ID = "123"), 
        // Guid.Parse will violently crash the server with an Exception (HTTP 500).
        // TryParse acts as a safety net: it attempts the conversion. If it fails, 
        // it safely returns 'false' so we can reject them gracefully (HTTP 401).
        if (string.IsNullOrWhiteSpace(customerIdString) ||
            !Guid.TryParse(customerIdString, out Guid customerId))
        {
            return Unauthorized();
        }

        // If we reach this line, we are 100% sure 'customerId' is a valid, safe Guid.
        await _transactionReversalService.CreateAsync(customerId, request);

        // Uses IActionResult to send a standardized HTTP 200 (Success) with a JSON message.
        return Ok(new
        {
            Message = "Reversal request submitted successfully."
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReversalResponse>>>
        Get()
    {
        var customerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(
                customerIdString) || !Guid.TryParse(customerIdString, out Guid customerId))
        {
            return Unauthorized();
        }

        var requests =
            await _transactionReversalService
                .GetAsync(customerId);

        return Ok(requests);
    }
}
using FinancialSolution.Application.DTOs.Reversal;
using FinancialSolution.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialSolution.API.Controllers;

[ApiController]
[Route("api/admin/reversals")]
[Authorize(Roles = "Admin")]
public class AdminReversalController : ControllerBase
{
    private readonly ITransactionReversalService
        _transactionReversalService;

    public AdminReversalController(
        ITransactionReversalService
            transactionReversalService)
    {
        _transactionReversalService =
            transactionReversalService;
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var adminIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // ARCHITECT'S NOTE: Guid.TryParse vs Guid.Parse
        // We NEVER use Guid.Parse() on data coming from the internet. 
        // If the JWT token is corrupted or tampered with (e.g., ID = "123"), 
        // Guid.Parse will violently crash the server with an Exception (HTTP 500).
        // TryParse acts as a safety net: it attempts the conversion. If it fails, 
        // it safely returns 'false' so we can reject them gracefully (HTTP 401).
        if (string.IsNullOrWhiteSpace(adminIdString) ||
            !Guid.TryParse(adminIdString, out Guid adminId))
        {
            return Unauthorized();
        }

        var requests =
            await _transactionReversalService
                .GetPendingAsync(adminId);

        return Ok(requests);
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(
        Guid id)
    {
        var adminIdString =
            User.FindFirst(
                ClaimTypes.NameIdentifier)
                ?.Value;

        if (string.IsNullOrWhiteSpace(adminIdString) || !Guid.TryParse(adminIdString, out Guid adminId))
        {
            return Unauthorized();
        }

        await _transactionReversalService
            .ApproveAsync(adminId, id);

        return Ok(
            "Reversal request approved.");
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(
        Guid id,
        RejectReversalRequest request)
    {
        var adminIdString =
            User.FindFirst(
                ClaimTypes.NameIdentifier)
                ?.Value;

        if (string.IsNullOrWhiteSpace(adminIdString) || !Guid.TryParse(adminIdString, out Guid adminId))
        {
            return Unauthorized();
        }

        await _transactionReversalService
            .RejectAsync(adminId, id, request.Reason);

        return Ok(
            "Reversal request rejected.");
    }
}
using FinancialSolution.Application.DTOs.Transaction;
using FinancialSolution.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(
        ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [Authorize(Roles = "Customer,Admin")]
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(
        TransferRequest request)
    {
        var customerId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(customerId))
        {
            return Unauthorized();
        }

        var response =
            await _transactionService.TransferAsync(request, Guid.Parse(customerId!));

        return Ok(response);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
    int page = 1,
    int pageSize = 10)
    {
        var userId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var history =
            await _transactionService
                .GetTransactionHistoryAsync(
                    Guid.Parse(userId),
                    page,
                    pageSize);

        return Ok(history);
    }


    [HttpGet("statement")]
    public async Task<IActionResult> GetStatement(
    int page = 1,
    int pageSize = 10)
    {
        var userId =
            User.FindFirst(
                System.Security.Claims
                    .ClaimTypes.NameIdentifier)
                ?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var statement =
            await _transactionService
                .GetStatementAsync(
                    Guid.Parse(userId),
                    page,
                    pageSize);

        return Ok(statement);
    }
}
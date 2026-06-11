using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.DTOs.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinancialSolution.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("my-wallet")]
    public async Task<IActionResult> GetMyWallet()
    {
        var userId =
     User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var wallet =
            await _walletService.GetMyWalletAsync(Guid.Parse(userId));

        return Ok(wallet);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("fund")]
    public async Task<IActionResult> FundWallet(
    FundWalletRequest request)
    {
        await _walletService.FundWalletAsync(request);

        return Ok("Wallet funded successfully.");
    }
}
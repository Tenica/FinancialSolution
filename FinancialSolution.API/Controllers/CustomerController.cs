using FinancialSolution.Application.DTOs.Customer;
using FinancialSolution.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterCustomerRequest request)
    {
        var response =
            await _customerService.RegisterCustomerAsync(request);

        return Ok(response);
    }
}
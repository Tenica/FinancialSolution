using FinancialSolution.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(
        IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers =
            await _adminService.GetCustomersAsync();

        return Ok(customers);
    }


    [HttpGet("customers/{id:guid}")]
    public async Task<IActionResult> GetCustomer(
    Guid id)
    {
        var customer =
            await _adminService.GetCustomerByIdAsync(id);

        return Ok(customer);
    }

    [HttpPatch("customers/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateCustomer(Guid id)
    {
        await _adminService.DeactivateCustomerAsync(id);

        return Ok(new
        {
            Message = "Customer deactivated successfully."
        });
    }

    [HttpPatch("customers/{id:guid}/activate")]
    public async Task<IActionResult> ActivateCustomer(Guid id)
    {
        await _adminService.ActivateCustomerAsync(id);

        return Ok(new
        {
            Message = "Customer activated successfully."
        });
    }

    [HttpPatch("customers/{id:guid}/promote")]
    public async Task<IActionResult> PromoteCustomer(Guid id)
    {
        await _adminService.PromoteToAdminAsync(id);

        return Ok(new
        {
            Message = "Customer promoted to Admin successfully."
        });
    }

    [HttpPatch("customers/{id:guid}/demote")]
    public async Task<IActionResult> DemoteCustomer(Guid id)
    {
        await _adminService.DemoteToCustomerAsync(id);

        return Ok(new
        {
            Message = "Admin demoted to Customer successfully."
        });
    }


    [HttpGet("audit-logs")]
    public async Task<IActionResult> GetAuditLogs()
    {
        var logs =
            await _adminService.GetAuditLogsAsync();

        return Ok(logs);
    }

    [HttpGet("customers/{id:guid}/audit-logs")]
    public async Task<IActionResult> GetCustomerAuditLogs(
    Guid id)
    {
        var logs =
            await _adminService
                .GetCustomerAuditLogsAsync(id);

        return Ok(logs);
    }
}
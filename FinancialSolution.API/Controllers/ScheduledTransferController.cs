using FinancialSolution.Application.DTOs;
using FinancialSolution.Application.DTOs.ScheduledTransfer;
using FinancialSolution.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialSolution.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScheduledTransferController : ControllerBase
    {
        private readonly IScheduledTransferService
            _scheduledTransferService;

        public ScheduledTransferController(
            IScheduledTransferService scheduledTransferService)
        {
            _scheduledTransferService =
                scheduledTransferService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]
    CreateScheduledTransfer request)
        {
            var customerId =
                User.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(customerId))
            {
                return Unauthorized();
            }

            var result =
                await _scheduledTransferService
                    .CreateAsync(
                        Guid.Parse(customerId),
                        request);

            return Ok(result);
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

            var result =
                await _scheduledTransferService
                    .GetAsync(
                        Guid.Parse(customerId));

            return Ok(result);
        }

    }



}

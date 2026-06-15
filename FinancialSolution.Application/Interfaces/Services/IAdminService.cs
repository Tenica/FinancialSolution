using FinancialSolution.Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<CustomerSummaryResponse>>
     GetCustomersAsync();

        Task<CustomerDetailResponse>
    GetCustomerByIdAsync(Guid customerId);

        Task DeactivateCustomerAsync(Guid customerId);

        Task ActivateCustomerAsync(Guid customerId);

        Task PromoteToAdminAsync(Guid customerId);

        Task DemoteToCustomerAsync(Guid customerId);

        Task<IEnumerable<AuditLogResponse>> GetAuditLogsAsync();

        Task<IEnumerable<CustomerAuditLogResponse>> GetCustomerAuditLogsAsync(Guid customerId);
    }


}

using FinancialSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.Interfaces.Repositories
{
    public interface IScheduledTransferRepository: IGenericRepository<ScheduledTransfer>
    {
        Task<List<ScheduledTransfer>>
            GetByCustomerIdAsync(Guid customerId);

        Task<List<ScheduledTransfer>>
    GetDueTransfersAsync();
    }
}

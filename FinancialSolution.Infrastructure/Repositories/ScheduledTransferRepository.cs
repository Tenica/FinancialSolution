using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Repositories
{
    /// <summary>
    /// Handles database operations specifically for Scheduled Transfers.
    /// </summary>
    // ARCHITECT'S NOTE: The (ApplicationDbContext context) is a C# 12 Primary Constructor.
    // The DI container automatically injects the context here, and we immediately pass it 
    // up to the GenericRepository base class using : GenericRepository<ScheduledTransfer>(context).
    public class ScheduledTransferRepository: GenericRepository<ScheduledTransfer>,
       IScheduledTransferRepository
    {

        public ScheduledTransferRepository(ApplicationDbContext context): base(context)
        {
          
        }

        /// <summary>
        /// Fetches the entire history of scheduled transfers for a specific user, sorted by newest first.
        /// </summary>
        /// <param name="customerId">The unique ID of the user.</param>
        /// <returns>A list of scheduled transfers.</returns>
       

        public async Task<List<ScheduledTransfer>>
            GetByCustomerIdAsync(Guid customerId)
        {
            // _dbSet is provided for free by the GenericRepository base class.
            // It acts as the direct gateway to the ScheduledTransfers SQL table
            return await _dbSet
                // ARCHITECT'S NOTE: PERFORMANCE ENGINE
                // Disables EF Core's Change Tracker because we are only reading data to display it.
                // This saves massive amounts of server CPU and RAM.
                .AsNoTracking()
                // Filters the SQL table to only grab rows matching this specific user.
                .Where(x => x.CustomerId == customerId)
                // Translates to 'ORDER BY CreatedAt DESC' in SQL so the frontend displays the newest items at the top
                .OrderByDescending(x => x.CreatedAt)
                // THE EXECUTIONER: Fires the actual SQL query over the network to the database 
                // and packages the resulting rows into a C# List.
                .ToListAsync();
        }

        public async Task<List<ScheduledTransfer>>
    GetDueTransfersAsync()
        {
            return await _dbSet
                .Where(x =>
                    x.IsActive &&
                    x.NextExecutionDate <= DateTime.UtcNow)
                .ToListAsync();
        }
    }
}



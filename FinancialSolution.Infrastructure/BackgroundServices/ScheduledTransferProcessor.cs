using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.Interfaces.UnitOfWork;
using FinancialSolution.Domain.Entities;
using FinancialSolution.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinancialSolution.Infrastructure.BackgroundServices;

public class ScheduledTransferProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
   

    public ScheduledTransferProcessor(
        IServiceScopeFactory scopeFactory)
    {
        Console.WriteLine(
       "ScheduledTransferProcessor created");

        _scopeFactory = scopeFactory;

      
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)

    {

 

        while (!stoppingToken.IsCancellationRequested)
        {  
            Console.WriteLine($"Processor running at {DateTime.UtcNow}");
           

            using var scope = _scopeFactory.CreateScope();

            var customerRepository =
  scope.ServiceProvider
      .GetRequiredService<
          ICustomerRepository>();

            var notificationService =
                scope.ServiceProvider
                    .GetRequiredService<
                        INotificationService>();

            var scheduledTransferRepository = 
                scope.ServiceProvider.GetRequiredService<IScheduledTransferRepository>();

            var transactionService =
                scope.ServiceProvider.GetRequiredService<ITransactionService>();

            var unitOfWork =
                scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var dueTransfers = await scheduledTransferRepository.GetDueTransfersAsync();

            Console.WriteLine(
    $"Found {dueTransfers.Count} due transfers");

           

            foreach (var transfer in dueTransfers)
            {
              
                try
                {

                    Console.WriteLine($"Executing transfer {transfer.Id}");

                    Console.WriteLine(
       $"Executing transfer {transfer.Id} - Amount: {transfer.Amount}");

                    var reference = Guid.NewGuid().ToString();

                    await transactionService
                        .ExecuteTransferAsync(
                            transfer.SenderAccountNumber,
                            transfer.ReceiverAccountNumber,
                            transfer.Amount,
                            reference);

                    var customer = await customerRepository.GetByIdAsync(transfer.CustomerId);

                    if (customer != null)
                    {
                        await notificationService
                            .SendScheduledTransferNotificationAsync(
                                customer.Email,
                                transfer.Amount,
                                transfer.SenderAccountNumber,
                                transfer.ReceiverAccountNumber,
                                reference);
                    }

                    transfer.LastExecutedAt =
                        DateTime.UtcNow;

                    Console.WriteLine(
 $"Request Date: {transfer.NextExecutionDate:o}");

                    transfer.LastTransactionReference = reference;

                    Console.WriteLine(
       "Transfer executed successfully");

                    if (transfer.RecurrenceType == RecurrenceType.OneTime)
                    {
                        transfer.IsActive = false;
                    }
                    else
                    {
                        transfer.NextExecutionDate =
                            CalculateNextExecutionDate(
                                transfer);
                    }

                    await unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    transfer.FailedAttemptCount++;

                    transfer.NextExecutionDate =
                        CalculateNextExecutionDate(transfer);

                    await unitOfWork.SaveChangesAsync();

                    Console.WriteLine(
                        $"Scheduled Transfer Failed: {ex.Message}");
                }
            }
            // We'll add logic here next

          

            await Task.Delay(
                TimeSpan.FromSeconds(10),
                stoppingToken);
        }
    }

    private static DateTime
    CalculateNextExecutionDate(
        ScheduledTransfer transfer)
    {
        return transfer.RecurrenceType switch
        {
            RecurrenceType.Daily =>
                transfer.NextExecutionDate.AddDays(1),

            RecurrenceType.Weekly =>
                transfer.NextExecutionDate.AddDays(7),

            RecurrenceType.Monthly =>
                transfer.NextExecutionDate.AddMonths(1),

            RecurrenceType.Yearly =>
                transfer.NextExecutionDate.AddYears(1),

            _ => transfer.NextExecutionDate
        };
    }
}
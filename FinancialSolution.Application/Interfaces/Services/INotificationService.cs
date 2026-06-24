using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendTransactionNotificationAsync(
        string email,
        decimal amount,
        string TransactionType,
        string reference);

    Task SendScheduledTransferNotificationAsync(
        string email,
        decimal amount,
        string senderAccountNumber,
        string receiverAccountNumber,
        string reference);

    Task SendPasswordChangedNotificationAsync(
        string email);

    Task SendReversalApprovedNotificationAsync(
     string email,
     string reference);

    Task SendReversalRejectedNotificationAsync(
    string email,
    string reference,
    string reason);
}

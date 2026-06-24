using FinancialSolution.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;

        public NotificationService(IEmailService emailService)
        {
               _emailService = emailService;
        }

        public async Task SendPasswordChangedNotificationAsync(string email)
        {
            var subject = "Password Changed Successfully";

            var body =
      $"""
        Dear Customer,

        Your password was changed successfully.

        If you did not perform this action,
        please contact support immediately.

        Financial Solution Bank
        """;

            await _emailService.SendAsync(
                email,
                subject,
                body);
        
        }

       


        public async Task SendScheduledTransferNotificationAsync(string email,
        decimal amount,
        string senderAccountNumber,
        string receiverAccountNumber,
        string reference)
        {
            var subject =
       "Scheduled Transfer Executed";

            var body =
                $"""
        Dear Customer,

        Your scheduled transfer has been executed successfully.

        Amount: {amount:C}

        From: {senderAccountNumber}

        To: {receiverAccountNumber}

        Date: {DateTime.UtcNow:dd MMM yyyy HH:mm}

        Ref: {reference}

        Financial Solution Bank
        """;

            await _emailService.SendAsync(
                email,
                subject,
                body);
        }

        public async Task  SendTransactionNotificationAsync(string email, decimal amount, string TransactionType, string reference)
        {
            var subject = "Transfer Successful";

            var body = $"""
        Dear Customer,

        Your {TransactionType} transfer of {amount:C}
        was completed successfully.

        Reference:
        {reference}

        Financial Solution Bank
        """;

            await _emailService.SendAsync(
       email,
       subject,
       body);
        }


        public async Task
    SendReversalApprovedNotificationAsync(
        string email,
        string transactionReference)
        {
            var subject =
                "Reversal Request Approved";

            var body =
                $"""
        Dear Customer,

        Your reversal request has been approved.

        Transaction Reference:
        {transactionReference}

        Our team will review the transaction
        for possible reversal.

        Financial Solution Bank
        """;

            await _emailService.SendAsync(
                email,
                subject,
                body);
        }

        public async Task
    SendReversalRejectedNotificationAsync(
        string email,
        string transactionReference, string reason)
        {
            var subject =
                "Reversal Request Rejected";

            var body =
                $"""
        Dear Customer,

        Your reversal request has been rejected.

        Transaction Reference:
        {transactionReference}

        Reason:
        {reason}

        If you require further clarification,
        please contact support.

        Financial Solution Bank
        """;

            await _emailService.SendAsync(
                email,
                subject,
                body);
        }
    }


}

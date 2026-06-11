using FinancialSolution.Application.DTOs.Transaction;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Application.DTOs.Common;


namespace FinancialSolution.Infrastructure.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    private readonly IWalletRepository _walletRepository;

    private readonly IAuditService _auditService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IWalletRepository walletRepository,
        IAuditService auditService)
    {
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _auditService = auditService;
    }

    public async Task<TransactionResponse> TransferAsync(
        TransferRequest request, Guid customerId)
    {
        var reference =
            Guid.NewGuid().ToString();

        await _transactionRepository.ExecuteTransferAsync(
            request.SenderAccountNumber,
            request.ReceiverAccountNumber,
            request.Amount,
            reference);

        await _auditService.LogAsync(
            customerId,
            "Transfer",
            $"Transferred {request.Amount} to account {request.ReceiverAccountNumber}",
            null);

        return new TransactionResponse
        {
            Reference = reference,
            Amount = request.Amount,
            Status = "Successful"
        };
    }

    public async Task<PagedResult<TransactionHistoryResponse>>
    GetTransactionHistoryAsync(
        Guid customerId,
        int page,
        int pageSize)
    {
        var wallet =
            await _walletRepository
                .GetByCustomerIdAsync(customerId);

        if (wallet == null)
        {
            throw new Exception("Wallet not found.");
        }

        var transactions =
            await _transactionRepository
                .GetWalletTransactionsAsync(
                    wallet.Id,
                    page,
                    pageSize);

        var totalCount =
            await _transactionRepository
                .GetTransactionCountAsync(wallet.Id);

        return new PagedResult<TransactionHistoryResponse>
        {
            Items = transactions
                .Select(x => new TransactionHistoryResponse
                {
                    Amount = x.Amount,

                    TransactionType =
                        x.TransactionType.ToString(),

                    Status =
                        x.Status.ToString(),

                    Reference = x.Reference,

                    CreatedAt = x.CreatedAt
                })
                .ToList(),

            Page = page,

            PageSize = pageSize,

            TotalCount = totalCount
        };
    }

    public async Task<StatementResponse>
    GetStatementAsync(
        Guid customerId,
        int page,
        int pageSize)
    {
        var wallet =
            await _walletRepository
                .GetByCustomerIdAsync(customerId);

        if (wallet == null)
        {
            throw new Exception("Wallet not found.");
        }

        var transactions =
            await _transactionRepository
                .GetWalletTransactionsAsync(
                    wallet.Id,
                    page,
                    pageSize);

        return new StatementResponse
        {
            AccountNumber =
                wallet.AccountNumber,

            Currency =
                wallet.Currency.Code,

            CurrentBalance =
                wallet.Balance,

            Transactions =
                transactions
                    .Select(x =>
                        new TransactionHistoryResponse
                        {
                            Amount = x.Amount,

                            TransactionType =
                                x.TransactionType.ToString(),

                            Status =
                                x.Status.ToString(),

                            Reference =
                                x.Reference,

                            CreatedAt =
                                x.CreatedAt
                        })
                    .ToList()
        };
    }
}
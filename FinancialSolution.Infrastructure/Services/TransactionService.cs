using FinancialSolution.Application.DTOs.Common;
using FinancialSolution.Application.DTOs.Transaction;
using FinancialSolution.Application.Interfaces.Repositories;
using FinancialSolution.Application.Interfaces.Services;
using FinancialSolution.Infrastructure.Repositories;


namespace FinancialSolution.Infrastructure.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    private readonly IWalletRepository _walletRepository;

    private readonly ICustomerRepository _customerRepository;

    private readonly IAuditService _auditService;

    private readonly INotificationService _notificationService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IWalletRepository walletRepository,
        ICustomerRepository customerRepository,
        IAuditService auditService,
        INotificationService notificationService)
    {
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _customerRepository = customerRepository;
        _auditService = auditService;
        _notificationService = notificationService;
    }

    public async Task<TransactionResponse> TransferAsync(
    TransferRequest request,
    Guid customerId)
    {
        var customer =
            await _customerRepository.GetByIdAsync(customerId) ?? throw new Exception("Customer not found.");

        if (!customer.IsBvnVerified)
        {
            throw new Exception(
                "Please complete BVN verification before making transfers.");
        }

        var wallet = await _walletRepository.GetByCustomerIdAsync(customerId) ?? throw new Exception("Wallet not found.");

        if (wallet.AccountNumber != request.SenderAccountNumber )
        {
            throw new Exception("You can only transfer from your own account.");
        }

        var reference =
            Guid.NewGuid().ToString();
    

        await ExecuteTransferAsync(
     request.SenderAccountNumber,
     request.ReceiverAccountNumber,
     request.Amount,
     reference);

        await _auditService.LogAsync(
            customerId,
            "Transfer",
            $"Transferred {request.Amount} to account {request.ReceiverAccountNumber}",
            null);

        await _notificationService
    .SendTransactionNotificationAsync(
        customer.Email,
         request.Amount,
        "Transfer",
        reference);

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

    //ask if this should be internal
    public async Task ExecuteTransferAsync(
    string senderAccountNumber,
    string receiverAccountNumber,
    decimal amount,
    string reference)
    {

        await _transactionRepository
            .ExecuteTransferAsync(
                senderAccountNumber,
                receiverAccountNumber,
                amount,
                reference);
    }
}
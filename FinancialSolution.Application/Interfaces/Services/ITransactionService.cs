using FinancialSolution.Application.DTOs.Transaction;
using FinancialSolution.Application.DTOs.Common;

namespace FinancialSolution.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<TransactionResponse> TransferAsync(
        TransferRequest request, Guid customerId);

    Task<PagedResult<TransactionHistoryResponse>>
     GetTransactionHistoryAsync(
         Guid customerId,
         int page,
         int pageSize);

    Task<StatementResponse>
    GetStatementAsync(
        Guid customerId,
        int page,
        int pageSize);

    Task ExecuteTransferAsync(
    string senderAccountNumber,
    string receiverAccountNumber,
    decimal amount,
    string reference
    );
}
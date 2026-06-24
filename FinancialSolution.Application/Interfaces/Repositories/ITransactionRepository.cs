using FinancialSolution.Domain.Entities;


public interface ITransactionRepository
{
    Task ExecuteTransferAsync(
        string senderAccountNumber,
        string receiverAccountNumber,
        decimal amount,
        string reference);

    Task<List<Transaction>> GetWalletTransactionsAsync(
    Guid walletId,
    int page,
    int pageSize);

  

    Task<int> GetTransactionCountAsync(
        Guid walletId);

    Task<Transaction?>
      GetByReferenceAsync(string reference);
}
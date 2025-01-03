using Transaction.Data.DTOs;

namespace Transactions.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Entities.Transaction>> GetByAccountId(string accountId);

    Task<int> CreateTransaction(TransactionDto transactionDto);
    
    Task<int> DeleteTransaction(Guid id);
}
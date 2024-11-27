namespace Transactions.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Entities.Transaction>> GetByAccountId(Guid accountId);

}
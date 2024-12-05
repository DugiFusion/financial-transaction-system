using Microsoft.EntityFrameworkCore;
using Transaction.Data;
using Transactions.Entities.Enumerations;
using Transactions.Repositories.Interfaces;

namespace Transactions.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly TransactionContext _transactionContext;

    public TransactionRepository(TransactionContext transactionContext)
    {
        _transactionContext = transactionContext;
    }
    public async Task<IEnumerable<Entities.Transaction>> GetByAccountId(Guid accountId)
    {
        // string query = "SELECT * FROM Transactions";
        // var transactions = await _transactionContext.Transactions
        //     .FromSqlRaw(query)
        //     .ToListAsync();
        // return transactions;

        return new Entities.Transaction[]
        {
            new Entities.Transaction()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                CustomerId = Guid.NewGuid(),
                Amount = 100,
                CreatedDate = DateTime.Now,
                Type = TransactionType.Deposit,
                Note = "Initial deposit"
            },
            new Entities.Transaction()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                CustomerId = Guid.NewGuid(),
                Amount = 20,
                CreatedDate = DateTime.Now,
                Type = TransactionType.Withdraw,
                Note = "Initial withdraw"
            }
        };
        
        return await _transactionContext
            .Transactions
            //.Where(x => x.AccountId == accountId)
            .ToListAsync();
        //return new List<TransactionDto>();
    }
}
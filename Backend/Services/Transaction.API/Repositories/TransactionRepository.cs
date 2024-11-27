using Microsoft.EntityFrameworkCore;
using Transaction.Data;
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

        // return new Entities.Transaction[]
        // {
        //     new Entities.Transaction()
        //     {
        //         Id = Guid.NewGuid(),
        //         AccountId = Guid.Empty,
        //         Amount = 100,
        //         CreatedDate = DateTime.Now,
        //         Type = 1
        //     }
        // };
        
        return await _transactionContext
            .Transactions
            //.Where(x => x.AccountId == accountId)
            .ToListAsync();
        //return new List<TransactionDto>();
    }
}
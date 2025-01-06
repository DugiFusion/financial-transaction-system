using Microsoft.EntityFrameworkCore;
using Transaction.Data;
using Transaction.Data.DTOs;
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
    public async Task<IEnumerable<Entities.Transaction>> GetByAccountId(string accountId)
    {
        return await _transactionContext
            .Transactions
            //.Where(x => x.AccountId == accountId)
            .ToListAsync();
        //return new List<TransactionDto>();
    }

    public async Task<int> CreateTransaction(TransactionDto transactionDto)
    {
        var transaction = new Entities.Transaction()
        {
            Id = Guid.NewGuid(),
            AccountId = transactionDto.AccountId,
            CustomerId = transactionDto.CustomerId,
            Amount = transactionDto.Amount,
            CreatedDate = DateTime.Now,
            Type = transactionDto.Type,
            Note = transactionDto.Note
        };
        _transactionContext.Transactions.Add(transaction);
        return await _transactionContext.SaveChangesAsync();
    }
    
    public async Task<int> DeleteTransaction(Guid id)
    {
        var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(x => x.Id == id);
        if (transaction == null)
        {
            return 0;
        }
        _transactionContext.Transactions.Remove(transaction);
        return await _transactionContext.SaveChangesAsync();
    }
}
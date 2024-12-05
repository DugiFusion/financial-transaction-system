using Transactions.Entities.Enumerations;

namespace Transaction.Data.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public double Amount { get; set; }
    public TransactionType? Type { get; set; }
    public DateTime CreatedDate { get; set; }
    
}
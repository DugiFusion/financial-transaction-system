namespace Transactions.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public double Amount { get; set; }

    public int? Type { get; set; } // For now it is just int but it should be an enum (0-add, 1-withdraw)

    public DateTime? CreatedDate { get; set; }
}
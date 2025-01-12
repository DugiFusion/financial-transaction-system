using System.ComponentModel.DataAnnotations.Schema;
using Transactions.Entities.Enumerations;

namespace Transactions.Entities;

public class Transaction
{
    [Column("id")] public Guid Id { get; set; }

    [Column("account_id")] public string AccountId { get; set; }

    [Column("customer_id")] public string CustomerId { get; set; }

    [Column("amount")] public double Amount { get; set; }

    [Column("note")] public string? Note { get; set; }

    [Column("type")] public TransactionType Type { get; set; }

    [Column("created_date")] public DateTime CreatedDate { get; set; }
}
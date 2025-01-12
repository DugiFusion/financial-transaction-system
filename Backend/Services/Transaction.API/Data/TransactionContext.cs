using Microsoft.EntityFrameworkCore;

namespace Transaction.Data;

public class TransactionContext : DbContext
{
    public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
    {
    }

    public DbSet<Transactions.Entities.Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transactions.Entities.Transaction>()
            .ToTable("Transactions", "dbo");
    }
}
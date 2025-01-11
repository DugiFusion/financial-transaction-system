using Microsoft.EntityFrameworkCore;
using Transactions.Entities;

namespace Transaction.Data;
public class TransactionContext : DbContext
    {
        public DbSet<Transactions.Entities.Transaction> Transactions { get; set; }

        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions.Entities.Transaction>()
                .ToTable("Transactions", "dbo");
        }
    }


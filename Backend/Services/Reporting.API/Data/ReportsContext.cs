using Microsoft.EntityFrameworkCore;
using Reporting.API.Entities;

namespace Reporting.API.Data;

public class ReportsContext : DbContext
{
    public ReportsContext(DbContextOptions<ReportsContext> options) : base(options)
    {
    }

    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Report>()
            .ToTable("Reports", "dbo");
    }
}
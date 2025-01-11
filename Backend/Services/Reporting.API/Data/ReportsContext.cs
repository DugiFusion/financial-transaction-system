using Microsoft.EntityFrameworkCore;
using Reporting.API.Entities;

namespace Reporting.API.Data;
public class ReportsContext : DbContext
{
    public DbSet<Reporting.API.Entities.Report> Reports { get; set; }

    public ReportsContext(DbContextOptions<ReportsContext> options) : base(options)
    {
            
            
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Report>()
            .ToTable("Reports", "dbo");
    }

}
using Microsoft.EntityFrameworkCore;
using Reporting.API.Entities;

namespace Transaction.Data;

public class ReportFilesContext:DbContext
{
    public DbSet<Reporting.API.Entities.ReportFile> ReportFiles { get; set; }
    public ReportFilesContext(DbContextOptions<ReportFilesContext> options) : base(options)
    {
            
            
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReportFile>()
            .ToTable("ReportFiles", "dbo");
    }
}
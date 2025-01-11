using Microsoft.EntityFrameworkCore;
using Reporting.API.Entities;

namespace Reporting.API.Data;

public class ReportFilesContext:DbContext
{
    public DbSet<ReportFile> ReportFiles { get; set; }
    public ReportFilesContext(DbContextOptions<ReportFilesContext> options) : base(options)
    {
            
            
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReportFile>()
            .ToTable("ReportFiles", "dbo");
    }
}
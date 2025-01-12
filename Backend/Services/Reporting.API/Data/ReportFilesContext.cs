using Microsoft.EntityFrameworkCore;
using Reporting.API.Entities;

namespace Reporting.API.Data;

public class ReportFilesContext : DbContext
{
    public ReportFilesContext(DbContextOptions<ReportFilesContext> options) : base(options)
    {
    }

    public DbSet<ReportFile> ReportFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReportFile>()
            .ToTable("ReportFiles", "dbo");
    }
}
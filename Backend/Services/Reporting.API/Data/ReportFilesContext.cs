using Microsoft.EntityFrameworkCore;

namespace Transaction.Data;

public class ReportFilesContext:DbContext
{
    public DbSet<Reporting.API.Entities.ReportFile> ReportFiles { get; set; }
    public ReportFilesContext(DbContextOptions<ReportFilesContext> options) : base(options)
    {
            
            
    }
}
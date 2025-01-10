using Microsoft.EntityFrameworkCore;
using Reporting.API.Entities;

namespace Reporting.API;
public class CombinedContext : DbContext
{
    public DbSet<Report> Reports { get; set; }
    public DbSet<ReportFile> ReportFiles { get; set; }

    public CombinedContext(DbContextOptions<CombinedContext> options) : base(options)
    {
    }
}

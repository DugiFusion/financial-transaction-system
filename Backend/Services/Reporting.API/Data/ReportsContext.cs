using Microsoft.EntityFrameworkCore;
using Reporting.API.Entities;

namespace Transaction.Data;
public class ReportsContext : DbContext
{
    public DbSet<Reporting.API.Entities.Report> Reports { get; set; }

    public ReportsContext(DbContextOptions<ReportsContext> options) : base(options)
    {
            
            
    }
        

}
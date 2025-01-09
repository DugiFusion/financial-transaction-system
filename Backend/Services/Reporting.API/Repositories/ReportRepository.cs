using System.Text;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Stream.Client.AMQP;
using Reporting.API.Entities;
using Reporting.API.Repositories.Interfaces;
using Transaction.Data;
using Transaction.Data.DTOs;

namespace Reporting.API.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly ReportsContext _reportsContext;

    public ReportRepository(ReportsContext reportsContext)
    {
        _reportsContext = reportsContext;
    }
     
    public async Task<IEnumerable<Report>> GetByAccountId(string accountId)
    {
        return await _reportsContext.Reports
            //.Where(x => x.AccountId == accountId)
            .ToListAsync();
    }

    // public async Task<int> CreateReport(TransactionDto[] reportDto)
    // {
    //     var newReport = new Entities.Report
    //     {
    //         AccountId = reportDto.First().AccountId,
    //         GeneratedAt = DateTime.UtcNow,
    //         FileName = $"{Guid.NewGuid()}.csv"
    //     };
    //
    //     var csvContent = new StringBuilder();
    //     csvContent.AppendLine("Id,AccountId,CustomerId,Amount,Type,Note,CreatedDate");
    //
    //     foreach (var transaction in reportDto)
    //     {
    //         csvContent.AppendLine($"{transaction.Id},{transaction.AccountId},{transaction.CustomerId},{transaction.Amount},{transaction.Type},{transaction.Note},{transaction.CreatedDate}");
    //     }
    //
    //     var filePath = Path.Combine("Reports", newReport.FileName);
    //     await File.WriteAllTextAsync(filePath, csvContent.ToString());
    //
    //     _reportsContext.Reports.Add(newReport);
    //     return await _reportsContext.SaveChangesAsync();
    // }

    public async Task<int> CreateReport(TransactionDto[] reportDto)
    {
        Guid reportId = Guid.NewGuid();
        var newReport = new Entities.Report
        {
            Id = reportId,
            AccountId = reportDto.First().AccountId,
            GeneratedAt = DateTime.UtcNow,
            FileName = $"{reportId}.csv"
        };

        var csvContent = new StringBuilder();
        csvContent.AppendLine("#,Id,AccountId,CustomerId,Amount,Type,Note,CreatedDate");

        int counter = 1;
        foreach (var transaction in reportDto)
        {
            csvContent.AppendLine($"{counter},{transaction.Id},{transaction.AccountId},{transaction.CustomerId},{transaction.Amount},{transaction.Type},{transaction.Note},{transaction.CreatedDate}");
            counter++;
        }

        // Specify the new directory path
        var directoryPath = Path.Combine("C:", "Reports");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var filePath = Path.Combine(directoryPath, newReport.FileName);
        await File.WriteAllTextAsync(filePath, csvContent.ToString());

        _reportsContext.Reports.Add(newReport);
        return await _reportsContext.SaveChangesAsync();
    }
    
    
    public async Task<int> DeleteReport(Guid id)
    {
        var report = await _reportsContext.Reports
            .FirstOrDefaultAsync(x => x.Id == id);

        if (report == null)
        {
            return 0;
        }

        _reportsContext.Reports.Remove(report);
        return await _reportsContext.SaveChangesAsync();
    }
}
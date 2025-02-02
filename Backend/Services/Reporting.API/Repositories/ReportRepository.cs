using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reporting.API.Data;
using Reporting.API.Data.DTOs;
using Reporting.API.Entities;
using Reporting.API.Repositories.Interfaces;
using TransactionDto = Reporting.API.Data.DTOs.TransactionDto;

namespace Reporting.API.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly CombinedContext _context;
    private readonly ILogger<ReportRepository> _logger;
    private readonly ReportFilesContext _reportFilesContext;
    private readonly ReportsContext _reportsContext;


    public ReportRepository(ReportsContext reportsContext, ReportFilesContext reportFilesContext,
        CombinedContext context, ILogger<ReportRepository> logger)
    {
        _reportsContext = reportsContext;
        _reportFilesContext = reportFilesContext;
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Report>> GetByAccountId(string accountId)
    {
        _logger.LogInformation("Repository request for account ID: {accountId}", accountId);
        return await _reportsContext.Reports
            .Where(x => x.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<IActionResult> GetFileByReportId(Guid reportId)
    {
        _logger.LogInformation("Repository request for report ID: {reportId}", reportId);
        var fileDto = await (from rf in _context.ReportFiles
            join r in _context.Reports on rf.ReportId equals r.Id
            where rf.ReportId == reportId
            select new FileDto
            {
                Name = r.FileName,
                Data = rf.FileData
            }).FirstOrDefaultAsync();

        // if (fileDto == null)
        // {
        //     return Results.NotFound();
        // }

        return new FileContentResult(fileDto.Data, "application/octet-stream")
        {
            FileDownloadName = fileDto.Name
        };
    }

    public async Task<int> CreateReport(TransactionDto[] reportDto)
    {
        _logger.LogInformation("Repository request for creating report!");

        var reportId = Guid.NewGuid();
        var newReport = new Report
        {
            Id = reportId,
            AccountId = reportDto.First().AccountId,
            GeneratedAt = DateTime.UtcNow,
            FileName = $"report_{reportId}"
        };

        var csvContent = new StringBuilder();
        csvContent.AppendLine("#,Id,AccountId,CustomerId,Amount,Type,Note,CreatedDate");

        var counter = 1;
        foreach (var transaction in reportDto)
        {
            csvContent.AppendLine(
                $"{counter},{transaction.Id},{transaction.AccountId},{transaction.CustomerId},{transaction.Amount},{transaction.Type},{transaction.Note},{transaction.CreatedDate}");
            counter++;
        }

        var csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());

        _reportsContext.Reports.Add(newReport);
        await _reportsContext.SaveChangesAsync();

        var reportFile = new ReportFile
        {
            Id = Guid.NewGuid(),
            ReportId = reportId,
            FileData = csvBytes
        };

        _reportFilesContext.ReportFiles.Add(reportFile);
        return await _reportFilesContext.SaveChangesAsync();
    }


    public async Task<int> DeleteReport(Guid id)
    {
        var report = await _reportsContext.Reports
            .FirstOrDefaultAsync(x => x.Id == id);

        if (report == null) return 0;

        _reportsContext.Reports.Remove(report);
        await _reportsContext.SaveChangesAsync();

        var reportFile = await _reportFilesContext.ReportFiles
            .FirstOrDefaultAsync(x => x.ReportId == id);
        if (reportFile == null) return 0;

        _reportFilesContext.ReportFiles.Remove(reportFile);
        return await _reportFilesContext.SaveChangesAsync();
    }
}
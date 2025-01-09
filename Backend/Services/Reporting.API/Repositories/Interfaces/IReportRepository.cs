using Microsoft.AspNetCore.Mvc;
using Reporting.API.Entities;
using Transaction.Data.DTOs;

namespace Reporting.API.Repositories.Interfaces;

public interface IReportRepository
{
    Task<IEnumerable<Report>> GetByAccountId(string accountId);
    Task<IActionResult> GetFileByReportId(Guid reportId);
    Task<int> CreateReport(TransactionDto[] reportDto);
    
    Task<int> DeleteReport(Guid id);
}
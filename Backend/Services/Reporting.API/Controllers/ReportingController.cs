using Microsoft.AspNetCore.Mvc;
using Reporting.API.Entities;
using Reporting.API.Repositories.Interfaces;

namespace Reporting.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ReportingController: ControllerBase
{
    private readonly IReportRepository _reportRepository;
    private readonly ILogger<ReportingController> _logger;

    
    public ReportingController(IReportRepository reportRepository, ILogger<ReportingController> logger)
    {
        _reportRepository = reportRepository;
        _logger = logger;
    }
    
    [HttpGet("{accountId}")]
    [ProducesResponseType(typeof(List<Report>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetByAccountId(string accountId)
    {
        _logger.LogInformation( "GetByAccountId requested for account ID: {accountId}", accountId);
        
        var result = await _reportRepository.GetByAccountId(accountId);
        return Ok(result);
    }
    
    [HttpGet("getFile/{reportId}")]
    [ProducesResponseType(typeof(List<Report>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetFileByReportId(Guid reportId)
    {
        _logger.LogInformation( "GetFileByReportId requested for report ID: {reportId}", reportId);

        var result = await _reportRepository.GetFileByReportId(reportId);
        

        return Ok(result);
    }
    
    
    
     
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        _logger.LogError( "DeleteTransaction requested for transaction ID: {id}", id);
        if (id == Guid.Empty)
        {
            _logger.LogError("Invalid transaction ID");
            return BadRequest("Invalid transaction ID");
        }

        var result = await _reportRepository.DeleteReport(id);
        if (result == 1)
        {
            return NoContent();
        }
            
        return NotFound();
            
    }
    
    
}
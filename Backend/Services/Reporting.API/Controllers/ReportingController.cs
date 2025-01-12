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
    
    /// <summary>
/// Retrieves a list of reports associated with the specified account ID.
/// </summary>
/// <param name="accountId">The unique identifier for the account whose reports are to be retrieved.</param>
/// <returns>A list of reports associated with the specified account ID.</returns>
/// <response code="200">Returns the list of reports for the specified account.</response>
/// <response code="400">The request is invalid due to a bad account ID.</response>
/// <response code="403">The user does not have permission to access the report data.</response>
/// <response code="404">The specified account was not found.</response>
/// <response code="500">An internal server error occurred while retrieving the reports.</response>
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

/// <summary>
/// Retrieves a file associated with the specified report ID.
/// </summary>
/// <param name="reportId">The unique identifier for the report whose file is to be retrieved.</param>
/// <returns>The file associated with the specified report.</returns>
/// <response code="200">Returns the file associated with the specified report ID.</response>
/// <response code="400">The request is invalid due to a bad report ID.</response>
/// <response code="403">The user does not have permission to access the report file.</response>
/// <response code="404">The specified report was not found.</response>
/// <response code="500">An internal server error occurred while retrieving the report file.</response>
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

/// <summary>
/// Deletes the report associated with the specified ID.
/// </summary>
/// <param name="id">The unique identifier for the report to be deleted.</param>
/// <returns>A response indicating whether the report was successfully deleted or not.</returns>
/// <response code="204">The report was successfully deleted.</response>
/// <response code="400">The request is invalid due to a bad report ID.</response>
/// <response code="404">The specified report was not found.</response>
/// <response code="500">An internal server error occurred while attempting to delete the report.</response>
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

/// <summary>
/// Health check for the service.
/// </summary>
/// <returns>Healthy message if healthy</returns>
[HttpGet("health")]
public IActionResult HealthCheck()
{
    return Ok("Healthy");
}

/// <summary>
/// Readiness check for the service.
/// </summary>
/// <returns>Ready message if allive</returns>

[HttpGet("readiness")]
public IActionResult ReadinessCheck()
{
    return Ok("Ready");
}
    
}
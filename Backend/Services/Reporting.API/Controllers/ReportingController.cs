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
        var result = await _reportRepository.GetByAccountId(accountId);
        return Ok(result);
    }
}
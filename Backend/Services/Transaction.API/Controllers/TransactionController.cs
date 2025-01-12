using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Transaction.Data.DTOs;
using Transactions.Entities.Requests;
using Transactions.EventBusProducer;
using Transactions.Repositories.Interfaces;

namespace Transactions.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    // Retry policy (retry 3 times with exponential backoff)
    private static readonly AsyncRetryPolicy _retryPolicy =
        Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    // Circuit Breaker policy (open the circuit after 3 failures in 30 seconds)
    private static readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy =
        Policy.Handle<Exception>()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));

    private readonly ILogger<TransactionController> _logger;
    private readonly Producer _producer;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionController(ITransactionRepository transactionRepository, ILogger<TransactionController> logger,
        Producer producer)
    {
        _transactionRepository = transactionRepository;
        _logger = logger;
        _producer = producer;
    }

    [HttpGet("{accountId}")]
    [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetByAccountId(string accountId)
    {
        try
        {
            // Apply retry and circuit breaker policies
            var result = await _retryPolicy.WrapAsync(_circuitBreakerPolicy)
                .ExecuteAsync(() => _transactionRepository.GetByAccountId(accountId));
            return Ok(result);
        }
        catch (BrokenCircuitException)
        {
            _logger.LogError("Circuit Breaker is open. Unable to process request.");
            return StatusCode(503, "Service is temporarily unavailable due to a circuit breaker.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching transactions.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transactionDto)
    {
        if (transactionDto == null)
        {
            _logger.LogError("Transaction data is null");
            return BadRequest("Transaction data is null");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Received transaction: {TransactionDto}", transactionDto);

        try
        {
            var result = await _retryPolicy.WrapAsync(_circuitBreakerPolicy)
                .ExecuteAsync(() => _transactionRepository.CreateTransaction(transactionDto));
            return Ok(result);
        }
        catch (BrokenCircuitException)
        {
            _logger.LogError("Circuit Breaker is open. Unable to process request.");
            return StatusCode(503, "Service is temporarily unavailable due to a circuit breaker.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the transaction.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("Invalid transaction ID");
            return BadRequest("Invalid transaction ID");
        }

        try
        {
            var result = await _retryPolicy.WrapAsync(_circuitBreakerPolicy)
                .ExecuteAsync(() => _transactionRepository.DeleteTransaction(id));
            if (result == 1) return NoContent();

            return NotFound();
        }
        catch (BrokenCircuitException)
        {
            _logger.LogError("Circuit Breaker is open. Unable to process request.");
            return StatusCode(503, "Service is temporarily unavailable due to a circuit breaker.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the transaction.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] TransactionRequest request)
    {
        if (request?.Transactions == null || request.Transactions.Count == 0)
            return BadRequest("Transaction data is null or empty");

        try
        {
            await _producer.SendMessage(request.Transactions, "transactionQueue");
            return Ok("Messages sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending messages.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("Healthy");
    }

    [HttpGet("readiness")]
    public IActionResult ReadinessCheck()
    {
        return Ok("Ready");
    }
}
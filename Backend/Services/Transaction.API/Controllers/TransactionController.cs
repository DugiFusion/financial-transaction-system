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

    
    /// <summary>
    /// Gets transactions by account ID.
    /// </summary>
    /// <param name="accountId">The account ID to filter transactions.</param>
    /// <returns>Returns a list of transactions for the given account.</returns>
    /// <response code="200">Returns list of transactions for account id.</response>
    /// <response code="400">The request is invalid due to a bad account id.</response>
    /// <response code="403">The user does not have permission to access the report data.</response>
    /// <response code="500">An internal server error occurred while retrieving the transactions.</response>
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

    /// <summary>
    /// Creates a new transaction.
    /// </summary>
    /// <param name="transactionDto">The transaction data to create a new transaction.</param>
    /// <returns>Returns 1 if transaction was created.</returns>
    /// <response code="200">Returns 1 if transaction was created.</response>
    /// <response code="400">The request is invalid due to a bad transaction.</response>
    /// <response code="403">The user does not have permission to access the report data.</response>
    /// <response code="500">An internal server error occurred while creating transaction.</response>
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

    /// <summary>
    /// Deletes an existing transaction by its ID.
    /// </summary>
    /// <param name="id">The transaction ID to delete.</param>
    /// <returns>Returns 204 No Content if deletion is successful, otherwise 404 Not Found.</returns>
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

    /// <summary>
    /// Sends transaction data as a message to the message queue.
    /// </summary>
    /// <param name="request">The transaction request containing the list of transactions to send.</param>
    /// <returns>Returns a status message indicating whether the messages were sent successfully.</returns>
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
using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using Transaction.Data.DTOs;
using Transactions.Entities.Enumerations;
using Transactions.Entities.Requests;
using Transactions.EventBusProducer;
using Transactions.Repositories;
using Transactions.Repositories.Interfaces;

namespace Transactions.Controllers;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionController> _logger;
        private readonly Producer _producer;

        public TransactionController(ITransactionRepository transactionRepository, ILogger<TransactionController> logger, Producer producer)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
            _producer = producer;
        }


        [HttpGet ("{accountId}")] 
        [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetByAccountId(string accountId)
        {
            var result = await _transactionRepository.GetByAccountId(accountId);
            return Ok(result);
        }
        
        
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            var result = await _transactionRepository.CreateTransaction(transactionDto);
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("Invalid transaction ID");
                return BadRequest("Invalid transaction ID");
            }

            var result = await _transactionRepository.DeleteTransaction(id);
            if (result == 1)
            {
                return NoContent();
            }
            
            return NotFound();
            
        }
        
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] TransactionRequest request)
        {
            if (request?.Transactions == null || request.Transactions.Count == 0)
            {
                return BadRequest("Transaction data is null or empty");
            }

            await _producer.SendMessage(request.Transactions, "transactionQueue");
            
            return Ok("Messages sent successfully");
        }
    }

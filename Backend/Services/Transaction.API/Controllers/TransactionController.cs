using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using Transaction.Data.DTOs;
using Transactions.Entities.Enumerations;
using Transactions.Repositories;
using Transactions.Repositories.Interfaces;

namespace Transactions.Controllers;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionRepository transactionRepository, ILogger<TransactionController> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
        }


        [HttpGet ("{accountId}")] 
        [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetByAccountId(Guid accountId)
        {
            var result = await _transactionRepository.GetByAccountId(accountId);
            return Ok(result);
        }
        
        
        [HttpPost]
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
            var result = await _transactionRepository.DeleteTransaction(id);
            return Ok(result);
        }
    }

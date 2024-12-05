using Microsoft.AspNetCore.Mvc;
using Transaction.Data.DTOs;
using Transactions.Repositories;
using Transactions.Repositories.Interfaces;

namespace Transactions.Controllers;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
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
    }

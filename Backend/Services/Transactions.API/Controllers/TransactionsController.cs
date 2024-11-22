using Microsoft.AspNetCore.Mvc;

namespace Transactions.Controllers;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        public TransactionsController()
        {
        }

        [HttpGet]
        // [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetByAccountId()
        {
            //   var result = await _transactionService.GetByAccountId(accountId);
            //  return Ok(result);
            return Ok("Transactions");
        }
    }

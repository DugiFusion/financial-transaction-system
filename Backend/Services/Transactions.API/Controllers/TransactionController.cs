using Microsoft.AspNetCore.Mvc;

namespace Transactions.Controllers;

public class TransactionController
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        public TransactionsController()
        {
        }

        // [HttpGet("{accountId}")]
        // // [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status403Forbidden)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesDefaultResponseType]
        // public async Task<ActionResult> GetByAccountId(Guid accountId)
        // {
        //     //   var result = await _transactionService.GetByAccountId(accountId);
        //     //  return Ok(result);
        //     throw NotImplementedException();
        // }
    }
}

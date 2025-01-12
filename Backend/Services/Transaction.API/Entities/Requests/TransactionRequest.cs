using Transaction.Data.DTOs;

namespace Transactions.Entities.Requests;

public class TransactionRequest
{
    public List<TransactionDto> Transactions { get; set; }
}
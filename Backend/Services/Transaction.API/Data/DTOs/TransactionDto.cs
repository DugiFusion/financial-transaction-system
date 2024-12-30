using System.Text.Json.Serialization;
using Transactions.Entities.Enumerations;

namespace Transaction.Data.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid CustomerId { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        [JsonConverter(typeof(TransactionTypeConverter))]

        public TransactionType Type { get; set; }
        public string? Note { get; set; }
    }
}
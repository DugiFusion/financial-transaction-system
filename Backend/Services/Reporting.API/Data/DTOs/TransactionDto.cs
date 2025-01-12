using System.Text.Json.Serialization;
using Reporting.API.Entities.Enumerations;

namespace Reporting.API.Data.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public string AccountId { get; set; } // PAN user id
    public string CustomerId { get; set; } // Customer PAN
    public double Amount { get; set; }
    public DateTime CreatedDate { get; set; }

    [JsonConverter(typeof(TransactionTypeConverter))]
    public TransactionType Type { get; set; }

    public string? Note { get; set; }
}
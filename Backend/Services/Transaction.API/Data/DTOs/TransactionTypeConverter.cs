using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Transactions.Entities.Enumerations
{
    public class TransactionTypeConverter : JsonConverter<TransactionType>
    {
        public override TransactionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "Deposit" => TransactionType.Deposit,
                "Withdraw" => TransactionType.Withdraw,
                "0" => TransactionType.Deposit,
                "1" => TransactionType.Withdraw,
                _ => throw new JsonException($"Invalid value for TransactionType: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, TransactionType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Transactions.Entities.Enumerations
// {
//     public class TransactionTypeConverter : JsonConverter<TransactionType>
//     {
//         public override TransactionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//         {
//             var value = reader.GetString();
//             return value switch
//             {
//                 "Deposit" => TransactionType.Deposit,
//                 "Withdraw" => TransactionType.Withdraw,
//                 "0" => TransactionType.Deposit,
//                 "1" => TransactionType.Withdraw,
//                 _ => throw new JsonException($"Invalid value for TransactionType: {value}")
//             };
//         }
//
//         public override void Write(Utf8JsonWriter writer, TransactionType value, JsonSerializerOptions options)
//         {
//             writer.WriteStringValue(value.ToString());
//         }
//     }
// }
{
    public class TransactionTypeConverter : JsonConverter<TransactionType>
    {
        public override TransactionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (Enum.TryParse<TransactionType>(stringValue, out var result))
                {
                    return result;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var intValue = reader.GetInt32();
                if (Enum.IsDefined(typeof(TransactionType), intValue))
                {
                    return (TransactionType)intValue;
                }
            }

            throw new JsonException($"Unable to convert {reader.GetString()} to TransactionType.");
        }

        public override void Write(Utf8JsonWriter writer, TransactionType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Reporting.API.Entities.Enumerations;

namespace Reporting.API.Data.DTOs;

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
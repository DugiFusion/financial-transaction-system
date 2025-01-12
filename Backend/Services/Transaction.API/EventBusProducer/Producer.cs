using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Transactions.EventBusProducer;

public class Producer
{
    private readonly IConnectionFactory _connectionFactory;

    // Constructor now uses dependency injection to get IConnectionFactory
    public Producer(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SendMessage<T>(T message, string queueName)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queueName, false, false, false,
            null);

        var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await channel.BasicPublishAsync("", queueName, messageBody);
    }
}
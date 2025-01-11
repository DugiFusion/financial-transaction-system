using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

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

        await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: messageBody);
    }

}

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Reporting.API.EventBusConsumer;

public class Consumer
{
    private readonly IConnectionFactory _connectionFactory;

    public Consumer(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async void StartConsuming(string queueName, Func<string, Task> handleMessage)
    {
        var connection = await _connectionFactory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queueName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await handleMessage(message);
        };

        await channel.BasicConsumeAsync(queueName, true, consumer);
    }
}
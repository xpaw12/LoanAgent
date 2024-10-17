using LoanAgent.Domain.Common;
using LoanAgent.Domain.Entities;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Text.Json;

namespace LoanAgent.Application.Common.Services;

public class LoanConsumer : IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _queueName;
    private bool _disposed = false;

    public LoanConsumer(IOptions<RabbitMqSettings> rabbitMqSettings)
    {
        var settings = rabbitMqSettings.Value;

        var factory = new ConnectionFactory()
        {
            HostName = settings.HostName,
            Port = settings.Port
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = settings.QueueName;

        _channel.QueueDeclare(queue: _queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void StartConsuming(Func<LoanEntity, Task> onMessageReceived)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var loan = JsonSerializer.Deserialize<LoanEntity>(message);

            if (loan != null)
            {
                await onMessageReceived(loan);
            }
        };

        _channel.BasicConsume(queue: _queueName,
                              autoAck: true,
                              consumer: consumer);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
                _channel.Dispose();
            }

            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        _disposed = true;
    }
}
using LoanAgent.Domain.Common;
using LoanAgent.Domain.Entities;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;

using System.Text;
using System.Text.Json;

namespace LoanAgent.Application.Common.Services;

public class LoanPublisher
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _queueName;

    public LoanPublisher(IOptions<RabbitMqSettings> rabbitMqSettings)
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

    public void PublishLoan(LoanEntity loan)
    {
        var message = JsonSerializer.Serialize(loan);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                              routingKey: _queueName,
                              basicProperties: null,
                              body: body);
    }
}

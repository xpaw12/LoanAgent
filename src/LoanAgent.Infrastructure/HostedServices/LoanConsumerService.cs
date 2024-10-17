using LoanAgent.Infrastructure.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using RabbitMQ.Client.Events;

using RabbitMQ.Client;

using System.Text.Json;

using System.Text;
using LoanAgent.Domain.Common;
using Microsoft.Extensions.Options;
using LoanAgent.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using LoanAgent.Infrastructure.SignalRHubs;
using System.Text.Json.Serialization;

namespace LoanAgent.Infrastructure.Jobs;


public class LoanConsumerService : IHostedService
{
    private readonly ILogger<LoanConsumerService> _logger;
    private readonly IHubContext<LoanHub> _hubContext;
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _queueName;
    private bool _disposed = false;

    public LoanConsumerService(IOptions<RabbitMqSettings> rabbitMqSettings, 
        ILogger<LoanConsumerService> logger,
        IHubContext<LoanHub> hubContext)
    {
        var settings = rabbitMqSettings.Value;

        var factory = new ConnectionFactory
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

        _logger = logger;

        _hubContext = hubContext;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var loan = JsonSerializer.Deserialize<LoanEntity>(message); 

                if (loan != null)
                {
                    await HandleLoanMessage(loan);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Failed to deserialize message: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: _queueName,
                              autoAck: true,
                              consumer: consumer);

        _logger.LogInformation("LoanConsumerService started and is listening for messages.");

        return Task.CompletedTask;
    }

    private async Task HandleLoanMessage(LoanEntity loan)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveLoanUpdate", loan);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();

        _logger.LogInformation("LoanConsumerService stopped.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _channel?.Dispose();
            _connection?.Dispose();
            _disposed = true;
        }
    }
}
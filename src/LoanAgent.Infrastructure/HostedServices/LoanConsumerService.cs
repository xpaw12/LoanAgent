using LoanAgent.Application.Common.Dtos;
using LoanAgent.Domain.Common;
using LoanAgent.Infrastructure.SignalRHubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Serilog;

using System.Text;
using System.Text.Json;

namespace LoanAgent.Infrastructure.Jobs;

public class LoanConsumerService : IHostedService
{
    private readonly IHubContext<LoanHub> _hubContext;
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _queueName;
    private bool _disposed = false;

    public LoanConsumerService(IOptions<RabbitMqSettings> rabbitMqSettings, 
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
                var loan = JsonSerializer.Deserialize<LoanDto>(message); 

                if (loan != null)
                {
                    Log.Information("Loan retreived from the queue: {@Loan}", loan);
                    await HandleLoanMessage(loan);
                }
            }
            catch (JsonException ex)
            {
                Log.Error($"Failed to deserialize message: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: _queueName,
                              autoAck: true,
                              consumer: consumer);

        Log.Information("LoanConsumerService started and is listening for messages.");

        return Task.CompletedTask;
    }

    private async Task HandleLoanMessage(LoanDto loan)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveLoanUpdate", loan);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();

        Log.Information("LoanConsumerService stopped.");

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
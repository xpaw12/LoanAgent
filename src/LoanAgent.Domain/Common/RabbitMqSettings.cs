namespace LoanAgent.Domain.Common;

public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string QueueName { get; set; } = "loansQueue";
}

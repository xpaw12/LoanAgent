using MediatR;

using Serilog;

using System.Text.Json;

namespace LoanAgent.Application.Common.Behaviours;

internal class LoggingBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehaviour()
    {
        _logger = Log.ForContext<TRequest>();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var requestBody = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });

        _logger.Information("Request '{RequestName}' was sent. Body: {@RequestBody}", requestName, requestBody);

        try
        {
            var response = await next();

            var responseBody = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            _logger.Information("Response received for '{RequestName}'. Body: {@ResponseBody}", requestName, responseBody);

            return response;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Request '{RequestName}' failed. Error: {ErrorMessage}", requestName, ex.Message);
            throw;
        }
    }
}


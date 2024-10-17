using MediatR;

using Serilog;

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

        _logger.Information("Request: {Name}, Data: {@Request}", requestName, request);

        return await next();
    }
}


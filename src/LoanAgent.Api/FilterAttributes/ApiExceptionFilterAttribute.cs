﻿using LoanAgent.Application.Common.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Serilog;

using ILogger = Serilog.ILogger;

namespace LoanAgent.Api.FilterAttributes;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IWebHostEnvironment _environment;
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly ILogger _logger;

    public ApiExceptionFilterAttribute(
        IWebHostEnvironment environment)
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            {
                typeof(ValidationException), HandleValidationException
            },
            {
                typeof(NotFoundException), HandleNotFoundException
            },
            {
                typeof(InvalidOperationException), HandleInvalidOperationException
            }
        };

        _environment = environment;
        _logger = Log.ForContext<ApiExceptionFilterAttribute>();
    }

    public override void OnException(ExceptionContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();

        if (_exceptionHandlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context);
            context.ExceptionHandled = true;

            return;
        }

        var details = new ProblemDetails
        {
            Title = "Internal Server Error",
            Detail = "Internal Server Error"
        };

        var exceptionMessage = context.Exception.Message;

        while (context.Exception.InnerException is not null)
        {
            context.Exception = context.Exception.InnerException;
            exceptionMessage += $" -> {context.Exception.Message}";
        }

        if (!_environment.IsProduction())
            details.Detail = exceptionMessage;

        if (!_environment.IsDevelopment())
            _logger.Error(context.Exception,
                "Unhandled exception: " +
                "Message: {Message} ",
                exceptionMessage);

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private static void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors);

        context.Result = new BadRequestObjectResult(details);
    }

    private static void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Title = "Resource was not found",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);
    }

    private static void HandleInvalidOperationException(ExceptionContext context)
    {
        var exception = (InvalidOperationException)context.Exception;

        var details = new ProblemDetails
        {
            Title = "The request could not be processed",
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);
    }
}

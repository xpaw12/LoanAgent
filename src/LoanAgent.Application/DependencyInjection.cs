using FluentValidation;

using LoanAgent.Application.Common.Behaviours;
using LoanAgent.Application.Common.Helpers;
using LoanAgent.Application.Common.Interfaces.Helpers;
using LoanAgent.Application.Common.Security.Jwt;
using LoanAgent.Application.Common.Security.Jwt.Interfaces;
using LoanAgent.Application.Common.Security.Jwt.Options;
using LoanAgent.Application.Common.Services;
using LoanAgent.Domain.Common;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace LoanAgent.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddHttpContextAccessor();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        services.AddOptions(configuration);

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
        services.AddSingleton<LoanPublisher>();
        services.AddSingleton<LoanConsumer>();
    }

    public static void AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtTokenOptions>(options =>
            configuration.GetSection(JwtTokenOptions.Key).Bind(options));
    }
}
